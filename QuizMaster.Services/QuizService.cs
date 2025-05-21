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
        public async Task<IList<Quiz>> Find() /// Gets all quizzes
        {
            return await _dbContext.Quizzes.Include(q => q.User).Include(c => c.Category).ToListAsync();
        }

        public async Task<IList<Quiz>> FindWithQuestions() /// Gets all quizzes
        {
            return await _dbContext.Quizzes
                .Include(q => q.Questions) 
                .ToListAsync();
        }

        public async Task<Quiz?> Get(int id) /// Gets a specific quiz by id
        {
            return await _dbContext.Quizzes.Include(q => q.Questions).Include(c => c.Category).FirstOrDefaultAsync(q => q.Id == id);
        }


        public async Task<Quiz?> Create(Quiz quiz) /// Creates a new quiz
        {
            quiz.CreatedAt = DateTime.Now;
            await _dbContext.Quizzes.AddAsync(quiz);
            await _dbContext.SaveChangesAsync();
            return quiz;
        }



        public async Task<Quiz?> Update(int id, Quiz entity)
        {
            var tempQuiz = await Get(id);

            if (tempQuiz is null)
            {
                return null;
            }

            tempQuiz.Title = entity.Title;
            tempQuiz.Description = entity.Description;
            tempQuiz.CategoryId = entity.CategoryId;
            tempQuiz.CreatedAt = entity.CreatedAt;
            tempQuiz.UserId = entity.UserId;
            

            await _dbContext.SaveChangesAsync();
            return tempQuiz;
        }

        public async Task Delete(int id)
        {
            var quiz = await Get(id);

            if (quiz is null)
            {
                return;
            }
            

            _dbContext.Quizzes.Remove(quiz);

           await _dbContext.SaveChangesAsync();
        }
    }
}
