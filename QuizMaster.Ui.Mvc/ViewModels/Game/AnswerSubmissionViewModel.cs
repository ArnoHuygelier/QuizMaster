using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels.Game
{
    public class AnswerSubmissionViewModel
    {
        [Required]
        public int QuizId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int? SelectedAnswerId { get; set; }

        [Required]
        public int CorrectCount { get; set; }

        [Required]
        public int CurrentIndex { get; set; }

        [Required]
        public bool IsTimedOut { get; set; }
    }
}