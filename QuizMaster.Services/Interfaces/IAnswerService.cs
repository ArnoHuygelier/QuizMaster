using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<IEnumerable<Answer>> Find();
        Task<Answer?> Get(int id);
        Task<Answer> Create(Answer entity);
        Task<Answer?> Update(int id, Answer updated);
        Task<bool> Delete(int id);
        Task<bool> BulkDelete(List<int> questionsIds);
    }
}
