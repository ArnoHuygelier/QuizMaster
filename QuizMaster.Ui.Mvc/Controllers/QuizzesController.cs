using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class QuizzesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
