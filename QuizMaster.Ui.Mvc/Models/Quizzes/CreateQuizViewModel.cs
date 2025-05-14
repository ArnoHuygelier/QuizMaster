//using Microsoft.Build.Framework;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using QuizMaster.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.Models.Quizzes
{
    public class CreateQuizViewModel
    {
        
        

        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
    }
}
