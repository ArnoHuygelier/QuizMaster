using QuizMaster.Models;
using QuizMaster.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace QuizMaster.Services
{
    public class GameService
    {
        private readonly QuizMasterDbContext _context;

        public GameService(QuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<Quiz?> StartQuizAsync(int quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<Question?> GetQuestionAsync(int questionId)
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<bool> IsAnswerCorrectAsync(int questionId, int answerId)
        {
            var answer = await _context.Answers
                .FirstOrDefaultAsync(a => a.Id == answerId && a.QuestionId == questionId);
            return answer != null && answer.IsCorrect;
        }

        public async Task<int> FinishQuizAsync(int quizId, string userId, int correctCount)
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

            return correctCount;
        }
    }
}
