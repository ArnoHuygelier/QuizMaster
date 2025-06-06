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


        public async Task CheckForNewHints(string userId)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            //  Hint logic
            var flawlessCount = await _context.QuizResults
                .Include(q => q.Quiz)
                .ThenInclude(quiz => quiz.Questions)
                .Where(q => q.UserId == userId && q.CorrectCount == q.Quiz.Questions.Count)
                .CountAsync();

            
            if (flawlessCount > 0 && flawlessCount % 3 == 0 && user.Hints < 3)
            {
                user.Hints += 1;
            }

            await _context.SaveChangesAsync();
        }

    }
}
