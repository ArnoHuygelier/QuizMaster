using Microsoft.AspNetCore.Mvc;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.Models;

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

            //leaderboardViewModels = users.Select(x => new LeaderboardViewModel
            //{ 
            //    UserId = x.UserId,
            //    AvatarUrl = x.Avatar.AvatarUrl,
            //    UserName = x.UserName,
            //    Score = 2,
            //    CreatedAt = x.CreatedAt
            //});
            
            return View(users);
        }
    }
}