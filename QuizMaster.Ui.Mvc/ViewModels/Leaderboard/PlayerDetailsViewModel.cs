namespace QuizMaster.Ui.Mvc.ViewModels.Leaderboard
{
    public class PlayerDetailsViewModel
    {
        public string Title { get; set; }
        public int Score { get; set; }
        public int CorrectCount { get; set; }
        public int AmountOfQuestions { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}