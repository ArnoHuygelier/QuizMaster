using Microsoft.AspNetCore.Mvc.Rendering;
using QuizMaster.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels.Quizzes
{
    public class EditQuizViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public User User { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
