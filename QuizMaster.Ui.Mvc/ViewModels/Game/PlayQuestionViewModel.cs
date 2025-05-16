using QuizMaster.Models;

namespace QuizMaster.Ui.Mvc.ViewModels.Game
{
    public class PlayQuestionViewModel
    {
        public int QuizId { get; set; }
        public Question Question { get; set; }
        public int CurrentIndex { get; set; }
        public int TotalQuestions { get; set; }
    }
}
