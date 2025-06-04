using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services
{
	public class QuizResultService
	{
		private readonly QuizMasterDbContext _context;

		public QuizResultService(QuizMasterDbContext context)
		{
			_context = context;
		}

		public async Task<ICollection<QuizResult>> Find()
		{
			return await _context.QuizResults
				.Include(u => u.User)
				.Include(q => q.Quiz)
				.ToListAsync();
		}

		public async Task<ICollection<QuizResult>> GetQuizResultsByUserId(string userId)
		{
			return await _context.QuizResults
				.Include(u => u.User)
				.Include(b => b.Quiz)
				.Where(ub => ub.UserId == userId)
				.ToListAsync();
		}

		public async Task<QuizResult?> Get(int id)
		{
			return await _context.QuizResults
				.Include(u => u.User)
				.Include(b => b.Quiz)
				.FirstOrDefaultAsync(ub => ub.Id == id);
		}


		//return just the score for a quiz by a user to display on quizdetails page
        public async Task<int> GetQuizScoreByUserId(int quizId, string userId)
        {
            return await _context.QuizResults
                .Where(q => q.QuizId == quizId && q.UserId == userId).OrderByDescending(q => q.CorrectCount)
                .Select(q => q.CorrectCount).FirstOrDefaultAsync();
        }


        //return list of topscorers for specific quiz
        public async Task<List<QuizResult>> GetTopScorersByQuizId(int quizId)
        {
            return await _context.QuizResults.Include(q => q.User).ThenInclude(u => u.Avatar)
                .Where(qr => qr.QuizId == quizId)
                .OrderByDescending(qr => qr.Score)
                .Take(3)
                .ToListAsync();

        }
    }
}