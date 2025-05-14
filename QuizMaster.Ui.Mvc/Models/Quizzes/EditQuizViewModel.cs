using Microsoft.AspNetCore.Mvc.Rendering;
using QuizMaster.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.Models.Quizzes
{
    public class EditQuizViewModel
    {
        [Required]
        public int Id { get; set; }


        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
