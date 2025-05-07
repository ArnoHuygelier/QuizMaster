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

        public IList<Quiz> GetAll()
        {
            return _dbContext.Quizzes.Include(q => q.User).ToList();
        }

        public Quiz? GetById(int id)
        {
            return _dbContext.Quizzes.FirstOrDefault(q => q.Id == id);
        }

        public Quiz? Create(Quiz quiz)
        {
            _dbContext.Quizzes.Add(quiz);
            _dbContext.SaveChanges();
            return quiz;
        }
    }
}
