using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> Find();
        Task<User?> Get(string id);
        Task<User> Create(User entity);
        Task<User?> Update(string id, User updated);
        Task<bool> Delete(string id);
    }
}
