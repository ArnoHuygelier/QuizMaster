using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.Controllers.ControllerBases;
using System;

namespace QuizMaster.Ui.Mvc.Controllers
{
	[Authorize(Roles = "Admin")]
    public class QuizzesController : CrudController<Quiz>
    {
       
        private readonly QuizService _quizService;

        public QuizzesController(QuizService quizService)
        {
            _quizService = quizService;
        }

        
        public override async Task<IActionResult> Index()
        {
            var quizzes = await _quizService.Find();
            return View(quizzes);
        }

        public override async Task<IActionResult> Detail(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }

        public override IActionResult Create()
        {
            return View();
        }

        public override async Task<IActionResult> Create(Quiz entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }

            var createdQuiz = await _quizService.Create(entity);
            return RedirectToAction("Index");
        }

        public override async Task<IActionResult> Edit(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz is null)
            {
                return RedirectToAction("Index");
            }

            

            return View(quiz);
        }

        public override async Task<IActionResult> Edit(int id, Quiz entity)
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

        public override async Task<IActionResult> Delete(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz is null)
            {
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        public override async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _quizService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
