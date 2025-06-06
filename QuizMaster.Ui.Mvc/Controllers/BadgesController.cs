using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services.Interfaces;
using QuizMaster.Services.Services;
using QuizMaster.Ui.Mvc.ViewModels.Badges;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

public class BadgesController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IBadgeService _badgeService;

    public BadgesController(IBadgeService badgeService, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _badgeService = badgeService;
    }

    public async Task<IActionResult> Index()
    {
        var badges = await _badgeService.Find();

        var viewModel = new BadgesViewModel
        {
            Badges = badges.Select(b => new BadgeViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                ImageUrl = b.ImageUrl,
                NumberOfUsers = b.UserBadges?.Count ?? 0
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBadgeViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var imageName = vm.Name.Replace(" ", "").ToLower()+Path.GetExtension(vm.ImageFile.FileName);
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Badges", imageName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await vm.ImageFile.CopyToAsync(fileStream);
        }


        var badge = new Badge
        {
            Name = vm.Name,
            Description = vm.Description,
            ImageUrl = "/images/badges/" + imageName,
            Type = vm.Type,
            Threshold = vm.Threshold

        };

        await _badgeService.Create(badge);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var badge = await _badgeService.Get(id);
        if (badge == null)
            return RedirectToAction(nameof(Index));

        var vm = new EditBadgeViewModel
        {
            Id = badge.Id,
            Name = badge.Name,
            Description = badge.Description,
            ImageUrl = badge.ImageUrl,
            Type = badge.Type,
            Threshold = badge.Threshold
        };

        ViewData["Id"] = id;

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditBadgeViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Id"] = id;
            return View(vm);
        }

        string imageUrl = vm.ImageUrl;

        if (vm.ImageFile != null)
        {
            var imageName = vm.Name.Replace(" ", "").ToLower() + Path.GetExtension(vm.ImageFile.FileName);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/badges", imageName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await vm.ImageFile.CopyToAsync(stream);
            }

            imageUrl = "/images/badges/" + imageName;
        }

        var updatedBadge = new Badge
        {
            Id = id,
            Name = vm.Name,
            Description = vm.Description,
            ImageUrl = imageUrl,
            Type = vm.Type,
            Threshold = vm.Threshold
        };

        await _badgeService.Update(id, updatedBadge);

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _badgeService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
