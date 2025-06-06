using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IUserBadgeService
    {
        Task<ICollection<UserBadge>> Find();
        Task<ICollection<UserBadge>> GetUserBadgesByUserId(string userId);
        Task<UserBadge?> Get(int id);
        Task<UserBadge> Create(UserBadge entity);
        Task<UserBadge?> Update(int id, UserBadge updated);
        Task<bool> Delete(int id);
    }
}
