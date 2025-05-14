using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Categories;
using System;

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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] Category category)
        {
            if (!ModelState.IsValid)
            {
                return CreateView("Create", category);
            }

            await _categoryService.Create(category);

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