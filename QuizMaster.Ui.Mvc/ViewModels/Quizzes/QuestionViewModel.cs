using System.ComponentModel.DataAnnotations;
namespace QuizMaster.Ui.Mvc.ViewModels.Quizzes
{
    public class QuestionViewModel

    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one answer is required")]
        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();

        public int CorrectAnswerIndex { get; set; } = 0;




    }
}
