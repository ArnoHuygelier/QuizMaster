using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.Controllers.ControllerBases;
using System;
using QuizMaster.Services.Interfaces;
using QuizMaster.Ui.Mvc.Models.Quizzes;

namespace QuizMaster.Ui.Mvc.Controllers
{
	[Authorize(Roles = "Admin")]
    public class QuizzesController : Controller
    {
       
        private readonly QuizService _quizService;

        
        public QuizzesController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var quizzes = await _quizService.Find();
            return View(quizzes);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> Create(CreateQuizViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var quiz = new Quiz
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                CreatedAt = DateTime.Now, 
                UserId = viewModel.UserId ?? "default-user-id" 
            };

            var createdQuiz = await _quizService.Create(quiz);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz is null)
            {
                return RedirectToAction("Index");
            }

            

            return View(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Quiz entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }

            var updatedQuiz = await _quizService.Update(id, entity);
            if (updatedQuiz == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz is null)
            {
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _quizService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
