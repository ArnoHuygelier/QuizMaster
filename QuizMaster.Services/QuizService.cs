using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;

namespace QuizMaster.Services
{
    public class QuizService
    {
        private readonly QuizMasterDbContext _dbContext;
        public QuizService(QuizMasterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IList<Quiz> Find() /// Gets all quizzes
        {
            return _dbContext.Quizzes.Include(q => q.User).ToList();
        }
        public Quiz? Get(int id) /// Gets a specific quiz by id
        {
            return _dbContext.Quizzes.FirstOrDefault(q => q.Id == id);
        }
        public Quiz? Create(Quiz quiz) /// Creates a new quiz
        {
            _dbContext.Quizzes.Add(quiz);
            _dbContext.SaveChanges();
            return quiz;
        }
        public Quiz? Update(int id, Quiz quiz) /// Updates a specific quiz by id
        {
            var tempQuiz = Get(id);

            if(tempQuiz is null)
            {
                return null;
            }

            tempQuiz.Title = quiz.Title;
            tempQuiz.Description = quiz.Description;
            tempQuiz.Questions = quiz.Questions;
            tempQuiz.CreatedBy = quiz.CreatedBy;
            tempQuiz.CreatedByNavigation = quiz.CreatedByNavigation;

            _dbContext.SaveChanges();
            return tempQuiz;

        }
    }
}
