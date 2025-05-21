using QuizMaster.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using System.Runtime.CompilerServices;

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
                .Include(q => q.Quiz).Where(q => q.QuizId == quizId)
                .Include(q => q.Answers)
                .ToListAsync();
            return a;

        }

        /// <summary>
        /// Get the all the questions linked to a quiz, delete the ones that are not present in the viewmodel
        /// </summary>
        /// <returns></returns>
        public async Task<List<Question>> GetToBeDeletedQuestions(int quizId, List<Question> questionsToKeep)
        {
            var allQuestions = await _context.Questions.Include(x => x.Answers).Where(x => x.QuizId == quizId).ToListAsync();
            
            var QuestionsBeDeleted = allQuestions.Where(x => !questionsToKeep.Any(y => y.Id == x.Id)).ToList();

            return QuestionsBeDeleted;
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

        /// <summary>
        /// Get all the questions that need to be deleted
        /// </summary>
        /// <param name="questionsIds">the questions that need to be deleted</param>
        /// <returns></returns>
        public async Task<bool> BulkDelete(List<int> questionsIds)
        {
            _context.Questions.RemoveRange(_context.Questions.Where(a => questionsIds.Contains(a.Id)));
            await _context.SaveChangesAsync();

            return true;
        }
    }
}