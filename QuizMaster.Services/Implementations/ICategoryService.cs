using QuizMaster.Models;

namespace QuizMaster.Services
{
    public interface ICategoryService
    {
        Task<ICollection<Category>> Find();
        Task<Category?> Get(int id);
        Task<Category> Create(Category entity);
        Task<Category?> Update(int id, Category updated);
        Task<bool> Delete(int id);
    }
}
