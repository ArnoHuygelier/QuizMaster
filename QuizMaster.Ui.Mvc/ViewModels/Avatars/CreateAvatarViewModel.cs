using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Ui.Mvc.ViewModels.Avatars
{
	public class CreateAvatarViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public IFormFile AvatarImage { get; set; }

		public required string AvatarUrl { get; set; }

	}
}
