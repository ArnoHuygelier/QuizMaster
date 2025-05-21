namespace QuizMaster.Ui.Mvc.ViewModels.Avatars
{
	public class AvatarsViewModel
	{
		public ICollection<AvatarViewModel> Avatars { get; set; } = new List<AvatarViewModel>();
	}
}
