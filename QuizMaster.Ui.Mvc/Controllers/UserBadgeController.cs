using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services.Interfaces;
using QuizMaster.Services.Services;

namespace QuizMaster.Ui.Mvc.Controllers
{
	[Authorize]
	public class UserBadgeController : Controller
	{
		private readonly IUserBadgeService _badgeService;
		private readonly IUserService _userService;
		private readonly UserManager<User> _userManager;

		public UserBadgeController(IUserBadgeService badgeService, IUserService userService, UserManager<User> userManager)
		{
			_badgeService = badgeService;
			_userService = userService;
			_userManager = userManager;
		}

		public async Task<IActionResult> MyBadges()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Challenge();
			}

			var badgesByUserId = await _badgeService.GetUserBadgesByUserId(user.Id);
			return View(badgesByUserId);
		}
	}
}
