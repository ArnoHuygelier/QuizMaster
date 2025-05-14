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

        public async Task<Question?> Create(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
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
            question.Categories = updated.Categories;
            question.Answers = updated.Answers;
            

            await _context.SaveChangesAsync();
            return question;
        }

        // Verwijderen
        public async Task<bool> Delete(int id)
        {
            var question = await Get(id);
            if (question == null)
            {
                return false;
            }
                

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
