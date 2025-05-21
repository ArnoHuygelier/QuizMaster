using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Categories;
using System;
using QuizMaster.Ui.Mvc.ViewModels.Quizzes;

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
           await _categoryService.Delete(id);

            return RedirectToAction("Index");
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