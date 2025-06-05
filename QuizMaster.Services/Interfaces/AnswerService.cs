using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
	public class AnswerService : IAnswerService
    {
		private readonly QuizMasterDbContext _context;

		public AnswerService(QuizMasterDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Answer>> Find()
		{
			return await _context.Answers.ToListAsync();
		}

		public async Task<Answer?> Get(int id)
		{
			return await _context.Answers.FindAsync(id);
		}

		public async Task<Answer> Create(Answer entity)
		{
			_context.Answers.Add(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<Answer?> Update(int id, Answer updated)
		{
			var entity = await _context.Answers.FindAsync(id);
			if (entity == null) return null;

			entity.AnswerText = updated.AnswerText;
			entity.IsCorrect = updated.IsCorrect;

			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<bool> Delete(int id)
		{
			var entity = await _context.Answers.FindAsync(id);
			if (entity == null) return false;

			_context.Answers.Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}

        /// <summary>
        /// Get all the answers that need to be deleted by making a temporary questionIds list
        /// </summary>
        /// <param name="questionsIds">the questionsIds that need to be deleted</param>
        /// <returns></returns>
        public async Task<bool> BulkDelete(List<int> questionsIds)
        {
            _context.Answers.RemoveRange(_context.Answers.Where(a => questionsIds.Contains(a.QuestionId)));
            await _context.SaveChangesAsync();

            return true;
        }
    }
}