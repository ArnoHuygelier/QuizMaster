using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Ui.Mvc.ViewModels;

namespace QuizMaster.Ui.Mvc.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return View();
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
