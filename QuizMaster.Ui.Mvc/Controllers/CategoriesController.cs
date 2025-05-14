using Microsoft.AspNetCore.Mvc;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Categories;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.Find();

            var categoriesViewModel = new CategoriesViewModel
            {
                Categories = categories.ToList()
            };

            return View(categoriesViewModel);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
