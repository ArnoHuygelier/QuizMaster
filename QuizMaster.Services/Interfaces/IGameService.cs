using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Quiz>> Find();
        Task<Quiz?> Get(int quizId);
        Task<Question?> GetQuestion(int questionId);
        Task<bool> IsAnswerCorrect(int questionId, int answerId);
        Task<QuizResult> CreateResult(int quizId, string userId, int correctCount, int score);
        Task<Quiz> Create(Quiz quiz);
        Task<Quiz?> Update(int id, Quiz updated);
        Task<bool> Delete(int id);
        Task<QuizResult?> GetResult(int id);
    }
}
