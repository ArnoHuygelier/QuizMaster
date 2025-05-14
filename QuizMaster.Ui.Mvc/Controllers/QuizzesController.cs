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
        private readonly AnswerService _answerService;


        public QuizzesController(QuizService quizService, UserService userService, QuestionService questionService, AnswerService answerService)
        {
            _quizService = quizService;
            _userService = userService;
            _questionService = questionService;
            _answerService = answerService;
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


            var quiz = new Quiz
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId.Value,
                CreatedAt = DateTime.Now,
                UserId = userId
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

            
            var questions = await _questionService.GetQuestionsByQuizId(id);

            var questionViewModels = questions.Select(q =>
            {
                var answerList = q.Answers.ToList();
                return new QuestionViewModel
                {
                    Text = q.QuestionText,
                    Answers = answerList.Select(a => new AnswerViewModel
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect
                    }).ToList(),
                    CorrectAnswerIndex = answerList.FindIndex(a => a.IsCorrect)
                };
            }).ToList();

            var viewModel = new AddQuestionsViewModel
            {
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                Questions = questionViewModels
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
            {
                return NotFound();
            }


            foreach (var questionViewModel in viewModel.Questions)
            {
                if (questionViewModel.CorrectAnswerIndex < 0 ||
                    questionViewModel.CorrectAnswerIndex >= questionViewModel.Answers.Count)
                {
                    ModelState.AddModelError(string.Empty, "Each question must have one correct answer.");
                    return View(viewModel);
                }

                // Set the correct answer manually
                for (int i = 0; i < questionViewModel.Answers.Count; i++)
                {
                    questionViewModel.Answers[i].IsCorrect = (i == questionViewModel.CorrectAnswerIndex);
                }

                var question = new Question
                {
                    QuestionText = questionViewModel.Text,
                    Answers = questionViewModel.Answers.Select(a => new Answer
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                };
                await _questionService.AddQuestionToQuiz(viewModel.QuizId, question);
                
            
            
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
                UserId = quiz.UserId,
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
                Id = viewModel.Id,
                CategoryId = viewModel.CategoryId,
                Title = viewModel.Title,
                Description = viewModel.Description,
                UserId = viewModel.UserId,
                CreatedAt = viewModel.CreatedAt,
            };

            var updatedQuiz = await _quizService.Update(viewModel.Id, quiz);
            if (updatedQuiz == null)
            {
                return NotFound();
            }

            return RedirectToAction("AddQuestions", new { id = viewModel.Id });
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

