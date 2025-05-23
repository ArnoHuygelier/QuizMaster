using QuizMaster.Ui.Mvc.ViewModels.Leaderboard;

namespace QuizMaster.Ui.Mvc.ViewModels.Quizzes
{
    public class QuizDetailsViewModel
    {

        public int Id { get; set; }
        public required string Title { get; set; }

        public required string Description { get; set; }

        public int NumberOfQuestions { get; set; }

        public string? Category { get; set; }

        public string Created { get; set;  }

        public required string UserName { get; set; }

        public int UserScore { get; set; }

        public string? ImageUrl { get; set; }

        public List<LeaderboardViewModel> TopScorers { get; set; }
    }
}
