using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;
using QuizMaster.Ui.Mvc.Controllers.ControllerBases;
using System;
using QuizMaster.Services.Interfaces;
using QuizMaster.Ui.Mvc.Models.Quizzes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace QuizMaster.Ui.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuizzesController : Controller
    {

        private readonly QuizService _quizService;
        private readonly UserService _userService;
        private readonly QuestionService _questionService;


        public QuizzesController(QuizService quizService, UserService userService, QuestionService questionService)
        {
            _quizService = quizService;
            _userService = userService;
            _questionService = questionService;
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
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuizViewModel viewModel)
        {
            

            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync();
                return View(viewModel);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.Get(userId);
            var category = await _quizService.GetCategoryById(viewModel.CategoryId);
            var quiz = new Quiz
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                Category = category,
                CreatedAt = DateTime.Now,
                UserId = userId,
                User = user
            };

            var createdQuiz = await _quizService.Create(quiz);
            return RedirectToAction("AddQuestions", new { id = createdQuiz?.Id });

        }

        [HttpGet]
        public async Task<IActionResult> AddQuestions(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var viewModel = new AddQuestionsViewModel
            {
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddQuestions([FromForm] AddQuestionsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                
                return View(viewModel);
            }
            var quiz = await _quizService.Get(viewModel.QuizId);
            if (quiz == null)
                return NotFound();

            foreach (var questionVM in viewModel.Questions)
            {
                if (questionVM.Correct < 0 || questionVM.Correct >= questionVM.Answers.Count)
                {
                    ModelState.AddModelError(string.Empty, "A correct answer must be selected for each question.");
                    return View(viewModel);
                }
                var question = new Question
                {
                    QuestionText = questionVM.Text,
                    Quizzes = new List<Quiz> { quiz },
                    Answers = new List<Answer>()
                };

                for (int i = 0; i < questionVM.Answers.Count; i++)
                {
                    var answerVM = questionVM.Answers[i];
                    var answer = new Answer
                    {
                        AnswerText = answerVM.AnswerText,
                        IsCorrect = (i == questionVM.Correct),
                        Question = question
                    };
                    await _questionService.Create(question);
                }

                await _quizService.AddQuestionToQuiz(quiz.Id, question);
            }

            return RedirectToAction("Index");
        }






        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz == null)
            {
                return RedirectToAction("Index");
            }

            var viewModel = new EditQuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                CategoryId = quiz.CategoryId
            };

            await PopulateCategoriesAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditQuizViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync();
                return View(viewModel);
            }

            var quiz = new Quiz
            {
                
                Title = viewModel.Title,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                User = viewModel.User,
                Category = viewModel.Category,
                CreatedAt = viewModel.CreatedAt,
                UserId = viewModel.UserId
            };

            var updatedQuiz = await _quizService.Update(viewModel.Id, quiz);
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


        private async Task PopulateCategoriesAsync()
        {
            var categories = await _quizService.GetCategories();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
        }
    }
}

