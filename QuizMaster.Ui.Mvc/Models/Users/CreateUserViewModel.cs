using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.Models.Users
{
    public class CreateUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;  // Default to active
    }
}
