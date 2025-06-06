using QuizMaster.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels.Badges
{
    public class CreateBadgeViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public IFormFile ImageFile { get; set; }

        public string? ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Badge type is required")]
        public BadgeType Type { get; set; }

        [Required (ErrorMessage = "Threshold is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Threshold must be higher then 0")]
        public int Threshold { get; set; }
    }
}
