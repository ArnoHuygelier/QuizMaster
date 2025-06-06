using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IBadgeService
    {
        Task<ICollection<Badge>> Find();
        Task<Badge?> Get(int id);
        Task<Badge> Create(Badge entity);
        Task<Badge?> Update(int id, Badge updated);
        Task<bool> Delete(int id);
        Task<List<Badge>> CheckAndAssignBadges(string userId);
    }
}
