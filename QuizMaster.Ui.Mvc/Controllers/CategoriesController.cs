using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using QuizMaster.Models;
using QuizMaster.Services.Interfaces;
using QuizMaster.Services.Services;
using QuizMaster.Ui.Mvc.ViewModels.Categories;


namespace QuizMaster.Ui.Mvc.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.Find();

            var viewModel = new CategoriesViewModel()
            {
                Categories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    NumberOfQuizzes = c.Quizzes?.Count ?? 0
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return CreateView("Create");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Category category)
        {
            if (!ModelState.IsValid)
            {
                return CreateView("Create", category);
            }
            await _categoryService.Create(category);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var category = await _categoryService.Get(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            return CreateView("Edit", category);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] Category category)
        {
            if (!ModelState.IsValid)
            {
                return CreateView("Create", category);
            }

            await _categoryService.Update(id, category);

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.Delete(id);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        private IActionResult CreateView(string viewName, Category? category = null)
        {
            var categories = _categoryService.Find();

            if (category is null)
            {
                return View(viewName);
            }
            return View(viewName, category);
        }
    }
}