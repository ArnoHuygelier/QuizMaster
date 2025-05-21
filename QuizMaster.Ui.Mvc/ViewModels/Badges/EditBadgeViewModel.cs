using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels.Badges
{
    public class EditBadgeViewModel
    {
        public required int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
    }
}
