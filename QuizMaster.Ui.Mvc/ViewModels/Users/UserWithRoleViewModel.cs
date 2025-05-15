using QuizMaster.Models;

namespace QuizMaster.Ui.Mvc.ViewModels.Users
{
    public class UserWithRoleViewModel
    {
        public User User { get; set; }
        public string Role { get; set; }  // of List<string> als je meerdere rollen per user toelaat
    }
}
