using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IList<Quiz>> Find();
        Task<IList<Quiz>> FindQuizzesContainingQuestions();
        Task<IList<Quiz>> FindWithQuestions();
        Task<IList<Quiz>> FindQuizzesByCategory(int categoryId);
        Task<Quiz?> Get(int id);
        Task<Quiz?> GetWithUser(int id);
        Task<Quiz?> GetByTitle(string title);
        Task<Quiz?> Create(Quiz quiz);
        Task<Quiz?> Update(int id, Quiz entity);
        Task Delete(int id);
    }
}
