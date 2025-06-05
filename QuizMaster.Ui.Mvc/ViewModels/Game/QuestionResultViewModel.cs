namespace QuizMaster.Ui.Mvc.ViewModels.Game
{
    public class QuestionResultViewModel
    {
        
        public string QuestionText { get; set; }
        
        public string SelectedAnswerText { get; set; }
        
        public string CorrectAnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
