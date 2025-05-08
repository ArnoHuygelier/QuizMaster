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


            leaderboardViewModels = users.Select(x => new LeaderboardViewModel
            {
                UserId = x.Id,
                AvatarUrl = x.Avatar?.Url,
                UserName = "Leander",  //x.Name,
                Score = 2,
                BadgeNames = x.UserBadges.Select(x => x.Badge.ImageUrl).ToList(),
                CreatedAt = DateOnly.FromDateTime(DateTime.Now) //x.CreatedAt
            }).ToList();

            return View(users);
        }
    }
}