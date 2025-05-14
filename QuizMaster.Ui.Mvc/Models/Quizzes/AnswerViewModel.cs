using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.Models.Quizzes
{
    public class AnswerViewModel
    {
        [Required(ErrorMessage ="Answer Text is required")]
        public string AnswerText { get; set; } = string.Empty;

        [Required(ErrorMessage ="Please select a correct answer")]
        public bool IsCorrect { get; set; }
    }
}
