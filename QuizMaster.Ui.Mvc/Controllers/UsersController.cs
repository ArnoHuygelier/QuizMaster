using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Services.Interfaces;
using QuizMaster.Ui.Mvc.Controllers.ControllerBases;
using QuizMaster.Ui.Mvc.Models.Users;
using System.Threading.Tasks;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserService _userService;

        public UsersController(UserService service, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userService = service;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.Find();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create user object
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign role
                    if (await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }

                    return RedirectToAction("Index"); // Redirect to the user list
                }
            }

            // If validation failed, re-display the form
            return View(model);
        }

        [Route("{nickname}")]
        public async Task<IActionResult> Profile(string nickname)
        {
            if (string.IsNullOrEmpty(nickname))
            {
                return NotFound(); // Als er geen naam is, geef een foutmelding terug
            }

            User? user = await _userService.GetByNickname(nickname);
            if (user is null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string nickname)
        {
            var user = await _userService.GetByNickname(nickname);
            if (user is null) return RedirectToAction("Index");

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.Get(model.Id);
            if (user is null) return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.IsActive = model.IsActive;

            await _userService.Update(user.Id, user);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.Get(id);
            if (user is null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userService.Delete(id);

            return RedirectToAction("Index");
        }

    }
}
