using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> Find();
        Task<Question?> Get(int id);
        Task<Question?> GetQuestionWithAnswers(int id);
        Task<List<Question>> GetQuestionsByQuizId(int quizId);
        Task<List<Question>> GetToBeDeletedQuestions(int quizId, List<Question> questionsToKeep);
        Task<Question?> Create(Question question);
        Task<Question?> Update(int? id, Question updated);
        Task AddQuestionToQuiz(int quizId, Question question);
        Task<bool> Delete(int id);
        Task<bool> BulkDelete(List<int> questionsIds);
    }
}
