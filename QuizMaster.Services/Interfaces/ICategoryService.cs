using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
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
