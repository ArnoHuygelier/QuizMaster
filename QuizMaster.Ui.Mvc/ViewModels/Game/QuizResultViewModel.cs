using QuizMaster.Models;
using QuizMaster.Ui.Mvc.ViewModels.Badges;

namespace QuizMaster.Ui.Mvc.ViewModels.Game
{
    public class QuizResultViewModel
    {
        public int QuizId { get; set; }

        public string Title { get; set; }
        public int Score { get; set; }
        public int Total { get; set; }
        public List<QuestionResultViewModel> QuestionResults { get; set; } = new();

        public List<Badge> BadgesEarned { get; set; } = new();
    }
}
