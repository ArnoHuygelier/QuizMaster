using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using QuizMaster.Models;
using System.ComponentModel.DataAnnotations;
using QuizMaster.Ui.Mvc.Models.Quizzes.Interfaces;
using QuizMaster.Ui.Mvc.Helpers.Validation;

namespace QuizMaster.Ui.Mvc.ViewModels.Quizzes
{
    public class CreateQuizViewModel : IQuizFormViewModel
    {



        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public required string Description { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Image")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }

    }
}

