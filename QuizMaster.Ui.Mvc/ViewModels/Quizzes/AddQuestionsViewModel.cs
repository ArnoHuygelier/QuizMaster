using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.Models.Quizzes
{
    public class AddQuestionsViewModel
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "You must add at least one question")]
        [Required]
        public List<QuestionViewModel> Questions { get; set; } = new();

        public bool HasCorrectAnswer { get; set; }
    }
}
