using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models;
using QuizMaster.Services.Interfaces;
using QuizMaster.Services.Services;
using QuizMaster.Ui.Mvc.ViewModels.Quizzes;
using System.Security.Claims;


namespace QuizMaster.Ui.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuizzesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly ICategoryService _categoryService;

        public QuizzesController(IQuizService quizService, IQuestionService questionService, IAnswerService answerService, ICategoryService categoryService, IWebHostEnvironment env)
        {
            _quizService = quizService;
            _questionService = questionService;
            _answerService = answerService;
            _categoryService = categoryService;
            _env = env;
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.Find();
            return View();
        }


        /// <summary>
        /// Handles quiz creation including optional image upload.
        /// Ensures the quiz title is unique.
        /// Redirects to AddQuestions after Quiz creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuizViewModel viewModel)
        {


            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.Find();
                return View(viewModel);
            }


            //Check for duplicate quiz title
            var existingQuiz = await _quizService.GetByTitle(viewModel.Title);
            if (existingQuiz != null)
            {
                ModelState.AddModelError("Title", "A quiz with this title already exists.");
                ViewBag.Categories = await _categoryService.Find();
                return View(viewModel);
            }

            //If user uploaded an image => save it to the root folder and change name to title of quiz.
            //
            string? imageName = null;

            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                imageName = await SaveImageAsync(viewModel.ImageFile, viewModel.Title);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var quiz = new Quiz
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                UserId = userId,
                ImageUrl = imageName,
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
           
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var quiz = await _quizService.Get(viewModel.QuizId);
            if (quiz == null)
            {
                return NotFound();
            }

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
                CategoryId = quiz.CategoryId,
                CreatedAt = quiz.CreatedAt,
                ImageUrl = quiz.ImageUrl
            };

            ViewBag.Categories = await _categoryService.Find();
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditQuizViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.Find();
                return View(viewModel);
            }
            var existingQuiz = await _quizService.Get(viewModel.Id);
            if (existingQuiz == null)
            {
                return NotFound();
            }
            if (!string.Equals(existingQuiz.Title, viewModel.Title, StringComparison.OrdinalIgnoreCase))
            {
                var quizWithSameTitle = await _quizService.GetByTitle(viewModel.Title);
                if (quizWithSameTitle != null)
                {
                    ModelState.AddModelError("Title", "A quiz with this title already exists.");
                    ViewBag.Categories = await _categoryService.Find();
                    return View(viewModel);
                }
            }

            string? imageName = existingQuiz.ImageUrl;
            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                // Delete the old image
                if (!string.IsNullOrEmpty(existingQuiz.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, "images/quizimage", existingQuiz.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new image
                imageName = await SaveImageAsync(viewModel.ImageFile, existingQuiz.Title);
            }

            var quiz = new Quiz
            {
                Id = viewModel.Id,
                CategoryId = viewModel.CategoryId.Value,
                Title = viewModel.Title,
                Description = viewModel.Description,
                UserId = viewModel.UserId,
                CreatedAt = existingQuiz.CreatedAt,
                UpdatedAt = DateTime.Now,
                ImageUrl = imageName
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
                    ModelState.AddModelError("", $"Question with Id {qvm.QuestionId} could not be updated.");
                    return View(viewModel);
                }

                //Add the updated question to the list
                updatedQuestions.Add(returningQuestion);
            }


            //Update the quiz UpdatedAt
            quiz.UpdatedAt = DateTime.Now;
            var quizResponse = await _quizService.Update(viewModel.QuizId, quiz);

            if (quizResponse == null)
            {
                return NotFound();
            }



            //Get all questions that need to be deleted
            var questions = await _questionService.GetToBeDeletedQuestions(viewModel.QuizId, updatedQuestions);

            if (questions.Count() != 0)
            {
                //Get a list with all the questionsIds
                List<int> questionsIds = questions.Select(q => q.Id).ToList();

                //First delete the answers linked to a question
                await _answerService.BulkDelete(questionsIds);

                //Then delete questions
                await _questionService.BulkDelete(questionsIds);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var quiz = await _quizService.Get(id);

            if (quiz != null)
            {
                //List with all the questionsIds
                List<int> questionsIds = quiz.Questions.Select(q => q.Id).ToList();

                // Delete associated image file if it exists
                if (!string.IsNullOrEmpty(quiz.ImageUrl))
                {
                    var imagePath = Path.Combine(_env.WebRootPath, "images/quizimage", quiz.ImageUrl);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                //Delete the anwers then questions then quiz
                await _answerService.BulkDelete(questionsIds);
                await _questionService.BulkDelete(questionsIds);
                await _quizService.Delete(id);

                return RedirectToAction("Index");
            }

            return NotFound();
        }


        /// <summary>
        /// If user uploads an image for a quiz, put it in database and save the image under a consistent filename in wwwroot folder (via Environment).
        /// </summary>
        private async Task<string?> SaveImageAsync(IFormFile? imageFile, string title)
        {

            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }


            var imageName = title.Replace(" ", "").ToLower() + Path.GetExtension(imageFile.FileName);


            var filePath = Path.Combine(_env.WebRootPath, "images/quizimage", imageName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }


            return imageName;
        }
    }
}