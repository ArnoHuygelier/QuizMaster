using Microsoft.Identity.Client;

namespace QuizMaster.Ui.Mvc.ViewModels.Leaderboard
{
    public class LeaderboardViewModel
    {
        public int Rank { get; set; }
        public string UserId { get; set; }
        public required string AvatarUrl { get; set; }
        public required string UserName { get; set; }
        public int? Score { get; set; }
        public ICollection<string> BadgeUrls { get; set; }
    }
}