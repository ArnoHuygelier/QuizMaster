using Microsoft.AspNetCore.Identity;
using QuizMaster.Models;
using QuizMaster.Ui.Mvc.Constants;

namespace QuizMaster.Ui.Mvc.Helpers
{
	public class SeedDatabaseHelper
	{
		public static async Task SeedRolesAndAdmin(IServiceProvider service)
		{
			// Seed the roles
			var userManager = service.GetService<UserManager<User>>();
			var roleManager = service.GetService<RoleManager<IdentityRole>>();

			// Add the roles
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

			// creating admin
			var user = new User
			{
				UserName = "admin",
				Email = "admin@gmail.com",
				AvatarId = 1,
				IsActive = true,
				EmailConfirmed = true,
				PhoneNumberConfirmed = true
			};

			var userInDb = await userManager.FindByEmailAsync(user.Email);

			if (userInDb == null)
			{
				await userManager.CreateAsync(user, "Testing123!");
				await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
			}
		}
	}
}
