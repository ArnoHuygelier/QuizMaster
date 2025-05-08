using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.Controllers.ControllerBases;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class UsersController : CrudController<User,string>
    {

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public override async Task<IActionResult> Index()
        {
            var users = await _userService.Find();
            return View(users);
        }

        [HttpGet]
        public override async Task<IActionResult> Detail(string id)
        {
            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet]
        public override IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public override async Task<IActionResult> Create(User entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }

            var createdUser = await _userService.Create(entity);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public override async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.Get(id);
            if (user is null)
            {
                return RedirectToAction("Index");
            }



            return View(user);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(string id, User entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }

            var updatedUser = await _userService.Update(id, entity);
            if (updatedUser == null)
            {
                return NotFound();
            }

           

            return RedirectToAction("Index");
        }

        [HttpGet]
        public override async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.Get(id);
            if (user is null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public override async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userService.Delete(id);

            return RedirectToAction("Index");
        }

    }
}
