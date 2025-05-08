using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly QuizService _quizService;

        public QuizzesController(QuizService quizService)
        {
            _quizService = quizService;
        }


       
        public IActionResult Detail(int id)
        {
            var quiz = _quizService.GetById(id);
            return View(quiz);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Quiz quiz)
        {
            _quizService.Create(quiz);
            return RedirectToAction("index", "home");
        }

    }
}
