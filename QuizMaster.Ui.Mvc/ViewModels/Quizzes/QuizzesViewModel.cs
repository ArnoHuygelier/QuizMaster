using QuizMaster.Models;

namespace QuizMaster.Ui.Mvc.ViewModels.Quizzes
{
    public class QuizzesViewModel
    {
        public int Id { get; set; }
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
