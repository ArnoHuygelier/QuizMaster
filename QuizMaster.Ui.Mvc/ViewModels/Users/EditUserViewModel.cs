using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels.Users
{
    public class EditUserViewModel
    {
        public required string Id { get; set; }

        [Required(ErrorMessage = "Username isssssss required")]
        [MaxLength(50, ErrorMessage = "User name cannot exceed 50 characters")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Is active is required")]
        public required bool IsActive { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public required string Role { get; set; }
    }
}
