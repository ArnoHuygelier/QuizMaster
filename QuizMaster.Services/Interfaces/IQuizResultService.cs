using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IQuizResultService
    {
        Task<ICollection<QuizResult>> Find();
        Task<ICollection<QuizResult>> GetQuizResultsByUserId(string userId);
        Task<QuizResult?> Get(int id);
        Task<int> GetQuizScoreByUserId(int quizId, string userId);
        Task<List<QuizResult>> GetTopScorersByQuizId(int quizId);
    }
}
