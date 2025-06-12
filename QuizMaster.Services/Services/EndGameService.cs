using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using QuizMaster.Services.Interfaces;

namespace QuizMaster.Services.Services
{
    public class EndGameService : IEndGameService
    {
        private readonly QuizMasterDbContext _context;

        public EndGameService(QuizMasterDbContext context)
        {
            _context = context;
        }

        
        public async Task<(int quizCount, int flawlessCount)> GetQuizStatsAsync(string userId)
        {
            // Query all results for the user, including quiz and questions count
            var results = await _context.QuizResults
                .Where(q => q.UserId == userId)
                .Select(q => new { q.CorrectCount, QuestionCount = q.Quiz.Questions.Count })
                .ToListAsync();

            int quizCount = results.Count;
            int flawlessCount = results.Count(r => r.CorrectCount == r.QuestionCount);

            return (quizCount, flawlessCount);
        }
    }
}
