using QuizMaster.Models;

namespace QuizMaster.Ui.Mvc.ViewModels.Quizzes
{
    public class QuizzesViewModel
    {
        public int Id { get; set; }
        
        public List<QuizViewModel> Quizzes { get; set; } = new List<QuizViewModel>();
    }
}
