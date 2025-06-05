using QuizMaster.Models;

namespace QuizMaster.Services
{
    public interface IBadgeService
    {
        Task<ICollection<Badge>> Find();
        Task<Badge?> Get(int id);
        Task<Badge> Create(Badge entity);
        Task<Badge?> Update(int id, Badge updated);
        Task<bool> Delete(int id);
        Task CheckAndAssignBadgesAsync(string userId);
    }
}
