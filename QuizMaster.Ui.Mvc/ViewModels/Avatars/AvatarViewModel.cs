using QuizMaster.Models;

namespace QuizMaster.Ui.Mvc.ViewModels.Avatars
{
	public class AvatarViewModel
	{

		public int Id { get; set; }
		public required string Name { get; set; }
		public required string AvatarUrl { get; set; }
		public int UserCount { get; set; }
	}
}
