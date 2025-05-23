using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;

namespace QuizMaster.Ui.Mvc.Controllers
{
	[Authorize]
	public class QuizResultController : Controller
	{
		private readonly QuizResultService _quizResultService;
		private readonly UserManager<User> _userManager;

		public QuizResultController(QuizResultService quizResultService, UserManager<User> userManager)
		{
			_quizResultService = quizResultService;
			_userManager = userManager;
		}

		public async Task<IActionResult> MyQuizzes()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Challenge();
			}

			var badgesByUserId = await _quizResultService.GetQuizResultsByUserId(user.Id);
			return View(badgesByUserId);
		}
	}
}
