using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Game;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Text.Json;

namespace QuizMaster.Ui.Mvc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
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
                return RedirectToAction("Details", "Quiz", new { id = quiz.Id });
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
                QuestionId = currentQuestion.Id,
                QuestionText = currentQuestion.QuestionText,
                SelectedAnswerId = selectedAnswerId,
                SelectedAnswerText = selectedAnswerText,
                CorrectAnswerId = correctAnswer?.Id ?? 0,
                CorrectAnswerText = correctAnswer?.AnswerText ?? "",
                IsCorrect = isCorrect
            });

            // Recalculate correct count
            int correctCount = answersSoFar.Count(a => a.IsCorrect);

            int nextIndex = submission.CurrentIndex + 1;

            // If last question answered, redirect to Finish and pass answers in TempData
            if (nextIndex >= questions.Count)
            {
                TempData["AnswersSoFar"] = JsonSerializer.Serialize(answersSoFar);
                return RedirectToAction("Finish", new { id = submission.QuizId, correctCount, submission.TotalTimeLeft});
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
            if (userId == null) return RedirectToAction("Login", "Account");

            // Get quiz info
            var quiz = await _gameService.Get(id);
            if (quiz == null)
            {
                Console.WriteLine($"Quiz with ID {id} not found");
                return RedirectToAction("Error", "Home");
            }

            // Create/save the quiz result record (optional, depending on your service)
            var result = await _gameService.CreateResult(id, userId, correctCount, totalTimeLeft);

            // Read AnswersSoFar from TempData and deserialize
            var answersJson = TempData["AnswersSoFar"] as string;
            List<QuestionResultViewModel> questionResults = new List<QuestionResultViewModel>();
            if (!string.IsNullOrEmpty(answersJson))
            {
                questionResults = JsonSerializer.Deserialize<List<QuestionResultViewModel>>(answersJson) ?? new List<QuestionResultViewModel>();
            }

            // Prepare view model
            var viewModel = new QuizResultViewModel
            {
                QuizId = id,
                Score = correctCount,
                Total = quiz.Questions.Count,
                QuestionResults = questionResults
                
            };

            return View("Result", viewModel);
        }
    }
}