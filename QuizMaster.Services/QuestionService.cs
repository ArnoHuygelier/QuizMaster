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

        public async Task<List<Question>> GetQuestionsByQuizId(int quizId)
        {
            var a = await _context.Questions
                .Include(q => q.QuizId == quizId)
                .Include(q => q.Answers)
                .ToListAsync();
            return a;

        }


        public async Task<Question?> Create(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        // Bijwerken
        public async Task<Question?> Update(int? id, Question updated)
        {

            //Get all questions
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id); 

            if (question == null)
                return null;


            // Update question fields
            question.QuestionText = updated.QuestionText;
            


            // Update existing answers and track changes
            foreach (var updatedAnswer in updated.Answers)
            {

                var existingAnswer = question.Answers.FirstOrDefault(a => a.Id == updatedAnswer.Id);

                if (existingAnswer != null)
                {
                    // Update existing
                    existingAnswer.AnswerText = updatedAnswer.AnswerText;
                    existingAnswer.IsCorrect = updatedAnswer.IsCorrect;
                }
                else
                {
                    // Add new answer
                    question.Answers.Add(new Answer
                    {
                        AnswerText = updatedAnswer.AnswerText,
                        IsCorrect = updatedAnswer.IsCorrect
                    });
                }
            }

            // Optionally remove deleted answers
            var updatedAnswerIds = updated.Answers.Where(a => a.Id != 0).Select(a => a.Id).ToList();
            var answersToRemove = question.Answers
                .Where(a => !updatedAnswerIds.Contains(a.Id))
                .ToList();

            foreach (var answer in answersToRemove)
            {
                _context.Answers.Remove(answer);
            }
            
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task AddQuestionToQuiz(int quizId, Question question)
        {

            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }


            quiz.Questions.Add(question);



            await _context.SaveChangesAsync();
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
