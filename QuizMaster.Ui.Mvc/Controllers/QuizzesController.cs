using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services;

using System;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using QuizMaster.Ui.Mvc.ViewModels.Quizzes;
using QuizMaster.Ui.Mvc.ViewModels.Categories;

namespace QuizMaster.Ui.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuizzesController : Controller
    {

        private readonly QuizService _quizService;
        private readonly QuestionService _questionService;
        private readonly AnswerService _answerService;
        private readonly CategoryService _categoryService;

        public QuizzesController(QuizService quizService, QuestionService questionService, AnswerService answerService, CategoryService categoryService)
        {
            _quizService = quizService;
            _questionService = questionService;
            _answerService = answerService;
            _categoryService = categoryService; 
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var quizzes = await _quizService.FindWithQuestions();

            var viewModel = new QuizzesViewModel()
            {
                Quizzes = quizzes.Select(q => new QuizViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    NumberOfQuestions = q.Questions?.Count ?? 0  
                }).ToList()
            };

            return View(viewModel);
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
            ViewBag.Categories = await _categoryService.Find();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuizViewModel viewModel)
        {
            

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.Find();
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
            if (quiz == null) return NotFound();

            var viewModel = new AddQuestionsViewModel
            {
                QuizId = quiz.Id,
                QuizTitle = quiz.Title
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddQuestions([FromForm] AddQuestionsViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var quiz = await _quizService.Get(viewModel.QuizId);
            if (quiz == null) return NotFound();

            foreach (var qvm in viewModel.Questions)
            {
                //Set the correct answer to true via the CorrectAnswerIndex
                qvm.Answers[qvm.CorrectAnswerIndex].IsCorrect = true;

                var question = new Question
                {
                    QuestionText = qvm.Text,
                    Answers = qvm.Answers.Select(a => new Answer
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

            ViewBag.Categories = await _categoryService.Find();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditQuizViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.Find();
                return View(viewModel);
            }

            var quiz = new Quiz
            {
                Id = viewModel.Id,
                CategoryId = viewModel.CategoryId.Value,
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

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> EditQuestions(int id)
        {
            var quiz = await _quizService.Get(id);
            if (quiz == null) return NotFound();

            var questions = await _questionService.GetQuestionsByQuizId(id);

            var viewModel = new EditQuestionsViewModel
            {
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                Questions = questions.Select(q =>
                {
                    var answers = q.Answers.ToList();
                    return new QuestionViewModel
                    {
                        QuestionId = q.Id,
                        Text = q.QuestionText,
                        Answers = answers.Select(a => new AnswerViewModel
                        {
                            Id = a.Id,
                            AnswerText = a.AnswerText,
                            IsCorrect = a.IsCorrect
                        }).ToList(),
                        CorrectAnswerIndex = answers.FindIndex(a => a.IsCorrect)
                    };
                }).ToList()
            };

            return View(viewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestions([FromForm] EditQuestionsViewModel viewModel)
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


            List<Question> updatedQuestions = new List<Question>();

            foreach (var qvm in viewModel.Questions)
            {
                Question returningQuestion;

                //Set the correct answer to true via the CorrectAnswerIndex
                qvm.Answers[qvm.CorrectAnswerIndex].IsCorrect = true;


                //Checks if the question is a new one or not
                if (qvm.QuestionId == 0)
                {
                    var newQuestion = new Question
                    {
                        QuizId = viewModel.QuizId,
                        QuestionText = qvm.Text,
                        Answers = qvm.Answers.Select(a => new Answer
                        {
                            Id = a.Id,
                            AnswerText = a.AnswerText,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    };

                    returningQuestion = await _questionService.Create(newQuestion);
                }
                else
                {
                    var updatedQuestion = new Question
                    {
                        Id = qvm.QuestionId,
                        QuestionText = qvm.Text,
                        Answers = qvm.Answers.Select(a => new Answer
                        {
                            Id = a.Id,
                            AnswerText = a.AnswerText,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    };

                    returningQuestion = await _questionService.Update(qvm.QuestionId, updatedQuestion);
                }


                if (returningQuestion == null)
                {
                    ModelState.AddModelError("", $"Question with Id {qvm.QuestionId} could need be updated.");
                    return View(viewModel);
                }

                //Add the updated question to the list
                updatedQuestions.Add(returningQuestion);
            }


            //Get all questions that need to be deleted
            var questions =  await _questionService.GetToBeDeletedQuestions(viewModel.QuizId, updatedQuestions);

            //Get a list with all the questionsIds
            List<int> questionsIds = questions.Select(q => q.Id).ToList();

            //First delete the answers linked to a question
            await _answerService.BulkDelete(questionsIds);

            //Then delete questions
            await _questionService.BulkDelete(questionsIds);


            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _quizService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}