using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels;
using QuizMaster.Ui.Mvc.ViewModels.Quizzes;

namespace QuizMaster.Ui.Mvc.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
    private readonly QuizService _quizService;

    public HomeController(ILogger<HomeController> logger, QuizService quizService)
	{
		_logger = logger;
        _quizService = quizService;
    }




    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeQuizViewmodel
        {
            Quizzes = await _quizService.Find()
        };

        return View(viewModel);
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
