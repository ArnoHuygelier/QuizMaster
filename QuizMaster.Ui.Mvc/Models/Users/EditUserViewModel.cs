using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.Models.Users
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
