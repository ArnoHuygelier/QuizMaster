using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Game;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

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
            var quiz = await _gameService.StartQuizAsync(id);
            if (quiz == null) return RedirectToAction("Index", "Home");

            var firstQuestion = quiz.Questions.OrderBy(q => q.Id).FirstOrDefault();
            if (firstQuestion == null) return RedirectToAction("Index", "Home");

            var viewModel = new PlayQuestionViewModel
            {
                QuizId = quiz.Id,
                Question = firstQuestion,
                CurrentIndex = 1,
                TotalQuestions = quiz.Questions.Count
            };

            // Reset score teller
            TempData["CorrectCount"] = 0;

            return View("Play", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Next([FromForm] AnswerSubmissionViewModel submission)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Start", new { id = submission.QuizId });

            var isCorrect = await _gameService.IsAnswerCorrectAsync(submission.QuestionId, submission.SelectedAnswerId);

            // Update correct count in TempData
            int correctCount = TempData.ContainsKey("CorrectCount") ? (int)TempData["CorrectCount"] : 0;
            if (isCorrect) correctCount++;
            TempData["CorrectCount"] = correctCount;

            var quiz = await _gameService.StartQuizAsync(submission.QuizId);
            var questions = quiz?.Questions.OrderBy(q => q.Id).ToList();
            int currentIndex = questions?.FindIndex(q => q.Id == submission.QuestionId) ?? -1;

            if (currentIndex + 1 >= questions?.Count)
            {
                // einde quiz
                return RedirectToAction("Finish", new { id = submission.QuizId });
            }

            var nextQuestion = questions![currentIndex + 1];
            var viewModel = new PlayQuestionViewModel
            {
                QuizId = submission.QuizId,
                Question = nextQuestion,
                CurrentIndex = currentIndex + 2,
                TotalQuestions = questions.Count
            };

            return View("Play", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Finish(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            int correctCount = TempData.ContainsKey("CorrectCount") ? (int)TempData["CorrectCount"] : 0;

            var score = await _gameService.FinishQuizAsync(id, userId, correctCount);

            var viewModel = new QuizResultViewModel
            {
                QuizId = id,
                Score = score,
                Total = await GetTotalQuestions(id)
            };

            return View("Result", viewModel);
        }

        private async Task<int> GetTotalQuestions(int quizId)
        {
            var quiz = await _gameService.StartQuizAsync(quizId);
            return quiz?.Questions.Count ?? 0;
        }
    }
}
