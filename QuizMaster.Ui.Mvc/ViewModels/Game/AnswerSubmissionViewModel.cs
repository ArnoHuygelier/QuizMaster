using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels.Game

{
    public class AnswerSubmissionViewModel
    {
        [Required]
        public int QuizId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int SelectedAnswerId { get; set; }
    }

}
