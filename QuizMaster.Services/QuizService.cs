using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IList<Quiz> Find()
        {
            return _dbContext.Quizzes.ToList();
        }

        public Quiz? Get(int id)
        {
            return _dbContext.Quizzes.FirstOrDefault(q => q.QuizId == id);
        }

        //public Quiz? Create(Quiz quiz)
        //{
        //    quiz.CreatedAt = DateTime.Now;
        //}
    }
}
