using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.ViewModels.Avatars;
using QuizMaster.Ui.Mvc.ViewModels.Categories;
using System.Threading.Tasks;

namespace QuizMaster.Ui.Mvc.Controllers
{
	public class AvatarController : Controller
	{
		private readonly AvatarService _avatarService;

		public AvatarController(AvatarService avatarService)
		{
			_avatarService = avatarService;
		}

		public async Task<IActionResult> Index()
		{
			var avatars = await _avatarService.Find();

			var viewModel = new AvatarsViewModel
			{
				Avatars = avatars.Select(c => new AvatarViewModel
				{
					Id = c.Id,
					Name = c.Name,
					AvatarUrl = c.AvatarUrl,
					UserCount = c.Users.Count
				}).ToList()

			};

			return View(viewModel);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateAvatarViewModel model)
		{
			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Avatars");
			if (!Directory.Exists(uploadsFolder))
			{
				Directory.CreateDirectory(uploadsFolder);
			}

			var fileName = Path.GetFileName(model.AvatarImage.FileName);
			var filePath = Path.Combine(uploadsFolder, fileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await model.AvatarImage.CopyToAsync(fileStream);
			}

			var avatar = new Avatar
			{
				Name = model.Name,
				AvatarUrl = "/images/Avatars/" + fileName
			};

			await _avatarService.Create(avatar);

			return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var avatar = await _avatarService.Get(id);
			if (avatar == null)
			{
				return NotFound();
			}

			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Avatars");
			var fileName = Path.GetFileName(avatar.AvatarUrl);
			var filePath = Path.Combine(uploadsFolder, fileName);

			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			await _avatarService.Delete(id);

			return RedirectToAction("Index");
		}

	}
}
