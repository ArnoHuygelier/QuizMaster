using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Game;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace QuizMaster.Ui.Mvc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameService _gameService;
        private readonly BadgeService _badgeService;
        private readonly UserService _userService;
        private readonly QuestionService _questionService;

        public GameController(GameService gameService, UserService userService, BadgeService badgeService, QuestionService questionService)
        {
            _gameService = gameService;
            _badgeService = badgeService;
            _userService = userService;
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> Start(int id)
        {
            var quiz = await _gameService.Get(id);
            if (quiz == null) return RedirectToAction("Index", "Home");

            var firstQuestion = quiz.Questions.OrderBy(q => q.Id).FirstOrDefault();
            if (firstQuestion == null)
            {
                // Zet TempData voor frontend feedback en redirect naar Details
                TempData["NoQuestions"] = true;
                return RedirectToAction("Details", "Home", new { id = quiz.Id });
            }

            var viewModel = new PlayQuestionViewModel
            {
                QuizId = quiz.Id,
                Question = firstQuestion,
                CurrentIndex = 0,
                TotalQuestions = quiz.Questions.Count,
                CorrectCount = 0,
                ImageUrl = quiz.ImageUrl,
                Title = quiz.Title
            };

            return View("Play", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Next([FromForm] AnswerSubmissionViewModel submission)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the quiz and questions
            var quiz = await _gameService.Get(submission.QuizId);
            if (quiz == null)
            {
                Console.WriteLine($"Quiz with ID {submission.QuizId} not found");
                return RedirectToAction("Error", "Home");
            }

            var questions = quiz.Questions?.OrderBy(q => q.Id).ToList();
            if (questions == null || !questions.Any())
            {
                Console.WriteLine($"No questions found for quiz {submission.QuizId}");
                return RedirectToAction("Error", "Home");
            }

            // Deserialize answers so far from JSON string in submission (or create new list)
            var answersSoFar = string.IsNullOrEmpty(submission.AnswersSoFarJson)
                ? new List<QuestionResultViewModel>()
                : JsonSerializer.Deserialize<List<QuestionResultViewModel>>(submission.AnswersSoFarJson) ?? new List<QuestionResultViewModel>();

            // Current question based on the submitted index
            var currentQuestion = questions[submission.CurrentIndex];
            var correctAnswer = currentQuestion.Answers.FirstOrDefault(a => a.IsCorrect);

            // Determine if user's selected answer is correct
            bool isCorrect = false;
            int? selectedAnswerId = submission.SelectedAnswerId;
            string selectedAnswerText = "";

            if (!submission.IsTimedOut && selectedAnswerId.HasValue)
            {
                var selectedAnswer = currentQuestion.Answers.FirstOrDefault(a => a.Id == selectedAnswerId.Value);
                if (selectedAnswer != null)
                {
                    selectedAnswerText = selectedAnswer.AnswerText;
                    isCorrect = selectedAnswer.IsCorrect;
                    if (isCorrect)
                    {
                        //Add the time left of this question to the total time left => to calculate the total score
                        submission.TotalTimeLeft += submission.TimeLeftInSeconds;
                        
                    }
                }
            }

            // Add current question result to answersSoFar
            answersSoFar.Add(new QuestionResultViewModel
            {
                QuestionText = currentQuestion.QuestionText,
                SelectedAnswerText = selectedAnswerText,
                CorrectAnswerText = correctAnswer?.AnswerText ?? "",
                IsCorrect = isCorrect
            });

            // Recalculate correct count
            int correctCount = answersSoFar.Count(a => a.IsCorrect);

            int nextIndex = submission.CurrentIndex + 1;

            // If last question answered, redirect to Finish and pass answers in TempData
            if (nextIndex >= questions.Count)
            {

                var score = submission.TotalTimeLeft * correctCount;
                var result = await _gameService.CreateResult(quiz.Id, userId, correctCount,score);
                //Update the score in aspNetUser table
                var user = await _userService.Get(userId);

                user.Score += score;

                var userResult = await _userService.Update(userId, user);
                TempData["AnsweredQuestions"] = JsonSerializer.Serialize(answersSoFar);
                return RedirectToAction("Finish", new { id = result.Id });
            }

            // Prepare next question view model with answers so far
            var nextQuestion = questions[nextIndex];
            var viewModel = new PlayQuestionViewModel
            {
                QuizId = quiz.Id,
                Question = nextQuestion,
                CurrentIndex = nextIndex,
                TotalQuestions = questions.Count,
                CorrectCount = correctCount,
                ImageUrl = quiz.ImageUrl,
                Title = quiz.Title,
                TotalTimeLeft = submission.TotalTimeLeft,
                AnswersSoFar = answersSoFar
            };

            return View("Play", viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Finish(int id, int totalTimeLeft, int correctCount = 0)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _gameService.GetResult(id);
            var quiz = await _gameService.Get(result.QuizId);
            if (quiz == null)
            {
                return RedirectToAction("Error");
            }

            //Calculation score
            //int score = totalTimeLeft * correctCount;

            //Create/save the quiz result record (optional, depending on your service)
            //var result = await _gameService.CreateResult(id, userId, correctCount, score);

            //Update the score in aspNetUser table
            //var user = await _userService.Get(userId);

            //user.Score += score;

            //var userResult = await _userService.Update(userId, user);


            // Read AnswersSoFar from TempData and deserialize
            //var answersJson = TempData["AnswersSoFar"] as string;

            //List<QuestionResultViewModel> questionResults = new List<QuestionResultViewModel>();
            //if (!string.IsNullOrEmpty(answersJson))
            //{
            //    questionResults = JsonSerializer.Deserialize<List<QuestionResultViewModel>>(answersJson) ?? new List<QuestionResultViewModel>();
            //}
            var newlyEarnedBadges = await _badgeService.CheckAndAssignBadges(userId);

            // Prepare view model

            // Deserialize the answers from TempData
            var answersJson = TempData["AnsweredQuestions"] as string;
            var questionResults = string.IsNullOrEmpty(answersJson)
                ? new List<QuestionResultViewModel>()
                : JsonSerializer.Deserialize<List<QuestionResultViewModel>>(answersJson);

            var viewModel = new QuizResultViewModel
            {
                QuizId = result.QuizId,
                Title = quiz.Title,
                Score = result.Score,
                Total = quiz.Questions?.Count ?? 0,
                UserId = result.UserId,
                QuestionResults = questionResults
            };

            // Assign badges
            viewModel.BadgesEarned = newlyEarnedBadges;

            return View("Result", viewModel);
        }


        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UseHint(PlayQuestionViewModel model)
        {
            var question = await _questionService.GetQuestionWithAnswers(model.Question.Id);

            if (question == null) return NotFound();

            var correct = question.Answers.FirstOrDefault(a => a.IsCorrect);
            var incorrect = question.Answers.Where(a => !a.IsCorrect).ToList();



            // Keep 1 incorrect and the correct
            var random = new Random();
            var randomIncorrect = incorrect.OrderBy(x => random.Next()).Take(1).ToList();
            var visibleAnswers = new List<int> { correct.Id, randomIncorrect[0].Id };
            TempData["AnswersSoFar"] = JsonSerializer.Serialize(model.AnswersSoFar);
            var viewModel = new PlayQuestionViewModel
            {
                QuizId = model.QuizId,
                Question = question,
                CurrentIndex = model.CurrentIndex,
                TotalQuestions = model.TotalQuestions,
                CorrectCount = model.CorrectCount,
                AnswersSoFar = string.IsNullOrEmpty(model.AnswersSoFarJson)
                    ? new List<QuestionResultViewModel>()
                    : JsonSerializer.Deserialize<List<QuestionResultViewModel>>(model.AnswersSoFarJson) ?? new List<QuestionResultViewModel>(),
                Title = model.Title,
                ImageUrl = model.ImageUrl,
                HintUsed = true,
                AnswerIdsToShow = visibleAnswers
            };

            return View("Play", viewModel);
        }

    }
}