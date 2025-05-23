using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;

namespace QuizMaster.Ui.Mvc.Controllers
{
	public class UserBadgeController : Controller
	{
		private readonly UserBadgeService _badgeService;
		private readonly UserService _userService;
		private readonly UserManager<User> _userManager;

		public UserBadgeController(UserBadgeService badgeService, UserService userService, UserManager<User> userManager)
		{
			_badgeService = badgeService;
			_userService = userService;
			_userManager = userManager;
		}

		public async Task<IActionResult> MyBadges(string userId)
		{
			var badgesByUserId = await _badgeService.GetUserBadgesByUserId(userId);
			if (badgesByUserId == null)
			{
				return NotFound();
			}
			return View(badgesByUserId);
		}

	}
}
