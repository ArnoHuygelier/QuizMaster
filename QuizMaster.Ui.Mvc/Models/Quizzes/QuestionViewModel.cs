using System.ComponentModel.DataAnnotations;
namespace QuizMaster.Ui.Mvc.Models.Quizzes
{
    public class QuestionViewModel
    {
        [Required(ErrorMessage = "Question text is required")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one answer is required")]
        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();

        //[Required(ErrorMessage = "A correct answer must be selected")]
        //public int Correct { get; set; } = -1;
    }
}
