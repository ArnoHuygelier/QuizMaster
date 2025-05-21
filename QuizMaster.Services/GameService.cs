using QuizMaster.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizMaster.Repository;

namespace QuizMaster.Services
{
    public class GameService
    {
        private readonly QuizMasterDbContext _context;

        public GameService(QuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<Quiz?> Get(int quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<Question?> GetQuestion(int questionId)
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<bool> IsAnswerCorrect(int questionId, int answerId)
        {
            var answer = await _context.Answers
                .FirstOrDefaultAsync(a => a.Id == answerId && a.QuestionId == questionId);
            return answer != null && answer.IsCorrect;
        }

        public async Task<QuizResult> CreateResult(int quizId, string userId, int correctCount)
        {
            var quizResult = new QuizResult
            {
                QuizId = quizId,
                UserId = userId,
                Score = correctCount,
                SubmittedAt = DateTime.UtcNow
            };

            _context.QuizResults.Add(quizResult);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.Score = (user.Score ?? 0) + correctCount;
            }

            await _context.SaveChangesAsync();
            return quizResult;
        }

        public async Task<IEnumerable<Quiz>> Find()
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .ToListAsync();
        }

        public async Task<Quiz> Create(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<Quiz?> Update(int id, Quiz updated)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return null;

            quiz.Title = updated.Title;
            quiz.Description = updated.Description;

            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool> Delete(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
