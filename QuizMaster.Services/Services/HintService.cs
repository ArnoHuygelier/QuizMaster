using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using QuizMaster.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Services
{
    public class HintService : IHintService
    {
        private readonly QuizMasterDbContext _context;

        public HintService(QuizMasterDbContext context)
        {
            _context = context;
        }


        public async Task<bool> CheckForNewHints(string userId)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return false;
            }

            var flawlessCount = await _context.QuizResults
                .Include(q => q.Quiz)
                .ThenInclude(quiz => quiz.Questions)
                .Where(q => q.UserId == userId && q.CorrectCount == q.Quiz.Questions.Count)
                .CountAsync();

            var lastQuizzes = await _context.QuizResults
                .Where(q => q.UserId == userId)
                .OrderByDescending(q => q.SubmittedAt)
                .Take(3)
                .ToListAsync();

            bool newHintAdded = false;
            if (user.Hints < 3 && CheckHintParameters(flawlessCount,lastQuizzes))
            {
                user.Hints += 1;
                newHintAdded = true;
            }


            await _context.SaveChangesAsync();
            return newHintAdded;

        }

        private bool CheckHintParameters(int flawlessCount, List<QuizResult> lastThreeQuizzes)
        {
            if (flawlessCount > 0 && flawlessCount % 3 == 0 || lastThreeQuizzes.All(q => q.Score >= 500))
            {
                return true;

            }

            return false;
        }

    }
}
