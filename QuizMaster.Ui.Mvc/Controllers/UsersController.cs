using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Ui.Mvc.Controllers.ControllerBases;

namespace QuizMaster.Ui.Mvc.Controllers
{
    public class UsersController : CrudController<User>
    {
        [HttpGet]
        public override Task<IActionResult> Index()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public override Task<IActionResult> Detail(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public override IActionResult Create()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public override Task<IActionResult> Create(User entity)
        {
            throw new NotImplementedException();
        }


        [HttpGet]
        public override Task<IActionResult> Edit(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public override Task<IActionResult> Edit(int id, User entity)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public override Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public override Task<IActionResult> DeleteConfirmed(int id)
        {
            throw new NotImplementedException();
        }

    }
}
