using QuizMaster.Models;

namespace QuizMaster.Services
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
