using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using QuizMaster.Services.Interfaces;

namespace QuizMaster.Services
{
    public class QuizService : ICrudService<Quiz>
    {
        private readonly QuizMasterDbContext _dbContext;
        public QuizService(QuizMasterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<Quiz>> Find() /// Gets all quizzes
        {
            return await _dbContext.Quizzes.Include(q => q.User).ToListAsync();
        }

        public async Task<Quiz?> Get(int id) /// Gets a specific quiz by id
        {
            return await _dbContext.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Quiz?> GetQuizzesWithQuestions(int id) /// Gets a specific quiz with questions by id
        {
            return await _dbContext.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<Quiz?> Create(Quiz entity) /// Creates a new quiz
        {
            entity.CreatedAt = DateTime.Now;
            await _dbContext.Quizzes.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<Quiz?> Update(int id, Quiz entity) /// Updates a specific quiz by id
        {
            var tempQuiz = await Get(id);

            if(tempQuiz is null)
            {
                return null;
            }

            tempQuiz.Title = entity.Title;
            tempQuiz.Description = entity.Description;
            tempQuiz.Questions = entity.Questions;
            

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
