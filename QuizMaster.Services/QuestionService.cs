using QuizMaster.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;

namespace QuizMaster.Services
{
    public class QuestionService
    {
        private readonly QuizMasterDbContext _context;

        public QuestionService(QuizMasterDbContext context)
        {
            _context = context;
        }

        // Alles opvragen
        public async Task<IEnumerable<Question>> Find()
        {
            return await _context.Questions.ToListAsync();
        }

        // Eén ding opvragen via ID
        public async Task<Question?> Get(int id)
        {
            return await _context.Questions.FindAsync(id);
        }

        // Bijwerken
        public async Task<Question?> Update(int id, Question updated)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return null;
            }
                

            // Velden handmatig bijwerken
            question.QuestionText = updated.QuestionText;
            
            // Voeg hier andere velden toe...

            await _context.SaveChangesAsync();
            return updated;
        }

        // Verwijderen
        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Questions.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
                

            _context.Questions.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
