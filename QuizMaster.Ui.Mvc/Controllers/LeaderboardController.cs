using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Leaderboard;
using System.Threading.Tasks;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly LeaderboardService _leaderboardService;
        private readonly QuizResultService _quizResultService;

        public LeaderboardController(LeaderboardService leaderboardService, QuizResultService quizResultService)
        {
            _leaderboardService = leaderboardService;
            _quizResultService = quizResultService;
        }

        [HttpGet]
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


        [HttpGet("Leaderboard/PlayerDetails/{id}")]
        public async Task<IActionResult> PlayerDetails([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var quizResults = await _quizResultService.GetQuizResultsByUserId(id);

            List<PlayerDetailsViewModel> playerDetails = quizResults.Select(x => new PlayerDetailsViewModel
            {
                Title = x.Quiz.Title,
                Score = x.Score,
                SubmittedAt = x.SubmittedAt
            }).ToList();

            return View(playerDetails);
        }
    }
}