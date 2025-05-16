using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels;
using QuizMaster.Ui.Mvc.ViewModels.Quizzes;

namespace QuizMaster.Ui.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly QuizService _quizService;
	private readonly CategoryService _categoryService;

    public HomeController(QuizService quizService, CategoryService categoryService)
	{
        _quizService = quizService;
		_categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var quizzes = await _quizService.Find();
		ViewData["Categories"] = await _categoryService.Find();
        return View(quizzes);
    }

	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var quiz = await _quizService.Get(id);
		return View(quiz);
	}

	public IActionResult Privacy()
	{
		return View();
	}

	public IActionResult Tos()
	{
		return View();
	}

	public IActionResult About()
	{
		return View();
	}

	[HttpGet]
	public IActionResult Contact()
	{
		return View(new ContactViewModel());
	}

	[HttpPost]
	public IActionResult Contact(ContactViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View("Contact", model);
		}

		TempData["SuccessMessage"] = "Message sent! We’ll get back to you soon.";

		return RedirectToAction("Contact");
	}
}
