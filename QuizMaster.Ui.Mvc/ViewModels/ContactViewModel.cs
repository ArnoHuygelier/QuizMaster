using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels
{

	public class ContactViewModel
	{
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Subject is required")]
		public string Subject { get; set; }

		[Required(ErrorMessage = "Message is required")]
		public string Message { get; set; }
	}
}
