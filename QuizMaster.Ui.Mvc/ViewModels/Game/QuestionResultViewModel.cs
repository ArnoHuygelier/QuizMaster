namespace QuizMaster.Ui.Mvc.ViewModels.Game
{
    public class QuestionResultViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int? SelectedAnswerId { get; set; }
        public string SelectedAnswerText { get; set; }
        public int CorrectAnswerId { get; set; }
        public string CorrectAnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
