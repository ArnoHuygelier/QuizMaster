using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface ICrudService<T, TId>
    {
        Task<IList<T>> Find(); 
        Task<T?> Get(TId id); 
        Task<T?> Create(T entity); 
        Task<T?> Update(TId id, T entity); 
        Task Delete(TId id); 
    }
}
