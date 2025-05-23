using Microsoft.AspNetCore.Mvc;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels;
using QuizMaster.Ui.Mvc.ViewModels.Quizzes;
using System.Diagnostics;
using System.Security.Claims;
using QuizMaster.Ui.Mvc.ViewModels.Leaderboard;

namespace QuizMaster.Ui.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly QuizService _quizService;
	private readonly CategoryService _categoryService;
    private readonly QuizResultService _quizResultService;
    private readonly LeaderboardService _leaderboardService;

    public HomeController(QuizService quizService, CategoryService categoryService, QuizResultService quizResultService, LeaderboardService leaderboardService)
    {
        _quizService = quizService;
        _categoryService = categoryService;
        _quizResultService = quizResultService;
        _leaderboardService = leaderboardService;
    }

    [HttpGet]
	public async Task<IActionResult> Index(int? categoryId)
	{
		var quizzes = categoryId.HasValue 
			? await _quizService.FindQuizzesByCategory(categoryId.Value)
			: await _quizService.FindQuizzesContainingQuestions();


		ViewData["Categories"] = await _categoryService.Find();
		ViewData["SelectedCategoryId"] = categoryId ?? 0;

		return View(quizzes);
	}


    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var quiz = await _quizService.GetWithUser(id);
        if (quiz == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        

        var topResults = await _quizResultService.GetTopScorersByQuizId(quiz.Id);
        List<LeaderboardViewModel> topScorers = new List<LeaderboardViewModel>();
        if (topResults.Count() > 0)
        {
            // Convert to LeaderboardViewModel
             topScorers = topResults
                .Select((result, index) => new LeaderboardViewModel
                {
                    Rank = index + 1,
                    UserId = result.UserId,
                    AvatarUrl = result.User.Avatar.AvatarUrl,
                    UserName = result.User.UserName,
                    Score = result.Score,
                    BadgeUrls = result.User.UserBadges
                        .Select(ub => ub.Badge.ImageUrl)
                        .ToList()
                })
                .ToList();
        }
        
        


        var viewModel = new QuizDetailsViewModel()
        {
            Title = quiz.Title,
            Description = quiz.Description,
            Category = quiz.Category?.Name ?? "No Category",
            Created = quiz.CreatedAt.ToString("d"),
            NumberOfQuestions = quiz.Questions.Count(),
            UserName = quiz.User.UserName,
            UserScore = await _quizResultService.GetQuizScoreByUserId(quiz.Id, userId),
			ImageUrl = quiz.ImageUrl,
			Id = quiz.Id,
           TopScorers = topScorers



        };

        if (quiz.Questions == null || !quiz.Questions.Any())
        {
            TempData["NoQuestions"] = true;
        }

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
