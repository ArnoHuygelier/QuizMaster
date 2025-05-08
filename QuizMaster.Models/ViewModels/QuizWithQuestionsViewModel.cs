using QuizMaster.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.ViewModels
{
    public class QuizWithQuestionsViewModel
    {
        public Quiz Quiz { get; set; } = new Quiz();

        [Required]
        public List<QuestionInputModel> Questions { get; set; } = new List<QuestionInputModel>();
    }

    public class QuestionInputModel
    {
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public List<string> Answers { get; set; } = new List<string> { "", "", "", "" };

        [Required]
        public int CorrectIndex { get; set; }
    }
}
