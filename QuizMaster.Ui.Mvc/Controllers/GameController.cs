using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Game;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System;

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
            Console.WriteLine($"Received submission - QuizId: {submission.QuizId}, QuestionId: {submission.QuestionId}, IsTimedOut: {submission.IsTimedOut}");

            int correctCount = submission.CorrectCount;

            if (!submission.IsTimedOut && submission.SelectedAnswerId.HasValue)
            {
                bool isCorrect = await _gameService.IsAnswerCorrect(submission.QuestionId, submission.SelectedAnswerId.Value);
                if (isCorrect) correctCount++;
            }

            var quiz = await _gameService.Get(submission.QuizId);
            if (quiz == null)
            {
                Console.WriteLine($"Quiz with ID {submission.QuizId} not found");
                return RedirectToAction("Error", "Home");
            }

            if (quiz.Questions == null || !quiz.Questions.Any())
            {
                Console.WriteLine($"No questions found for quiz {submission.QuizId}");
                return RedirectToAction("Error", "Home");
            }

            var questions = quiz.Questions.OrderBy(q => q.Id).ToList();
            int currentIndex = submission.CurrentIndex;

            Console.WriteLine($"Current index: {currentIndex}, Total questions: {questions.Count}");

            if (currentIndex + 1 >= questions.Count)
            {
                Console.WriteLine("All questions answered - redirecting to Finish");
                return RedirectToAction("Finish", new { id = submission.QuizId, correctCount });
            }

            var nextQuestion = questions[currentIndex + 1];

            var viewModel = new PlayQuestionViewModel
            {
                QuizId = submission.QuizId,
                Question = nextQuestion,
                CurrentIndex = currentIndex + 1,
                TotalQuestions = questions.Count,
                CorrectCount = correctCount,
                ImageUrl = quiz.ImageUrl,
                Title = quiz.Title
            };

            return View("Play", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Finish(int id, int correctCount = 0)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var result = await _gameService.CreateResult(id, userId, correctCount);
            var quiz = await _gameService.Get(id);

            var viewModel = new QuizResultViewModel
            {
                QuizId = id,
                Score = result.Score,
                Total = quiz?.Questions.Count ?? 0
            };

            return View("Result", viewModel);
        }
    }
}
