using Microsoft.AspNetCore.Mvc;
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


        //public IActionResult Index()
        //{
        //    return View();
        //}


    }
}
