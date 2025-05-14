using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
