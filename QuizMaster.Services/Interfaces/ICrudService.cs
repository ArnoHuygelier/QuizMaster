using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface ICrudService<T>
    {
        Task<IList<T>> Find(); 
        Task<T?> Get(int id); 
        Task<T?> Create(T entity); 
        Task<T?> Update(int id, T entity); 
        Task Delete(int id); 
    }
}
