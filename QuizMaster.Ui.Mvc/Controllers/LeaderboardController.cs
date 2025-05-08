using Microsoft.AspNetCore.Mvc;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Leaderboard;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly LeaderboardService _leaderboardService;

        public LeaderboardController(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        public IActionResult Index()
        {
            var users = _leaderboardService.Find();

            List<LeaderboardViewModel> leaderboardViewModels = new List<LeaderboardViewModel>();


            leaderboardViewModels = users.Select((x, index) => new LeaderboardViewModel
            {
                Rank = index + 1,
                UserId = x.Id,
                AvatarUrl = x.Avatar?.AvatarUrl,
                UserName = x.UserName,
                Score = x.Score,
                BadgeUrls = x.UserBadges.Select(x => x.Badge.ImageUrl).ToList()
            }).ToList();

            return View(leaderboardViewModels);
        }
    }
}