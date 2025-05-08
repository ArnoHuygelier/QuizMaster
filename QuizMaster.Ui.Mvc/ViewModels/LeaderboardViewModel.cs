using Microsoft.Identity.Client;

namespace QuizMaster.Ui.Mvc.Models
{
    public class LeaderboardViewModel
    {
        public int UserId { get; set; }
        public required string AvatarUrl { get; set; }
        public required string UserName { get; set; }
        public int Score { get; set; }
        
        public ICollection<string> BadgeNames { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}