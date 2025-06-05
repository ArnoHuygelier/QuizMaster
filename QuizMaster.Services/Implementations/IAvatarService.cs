using QuizMaster.Models;

namespace QuizMaster.Services
{
    public interface IAvatarService
    {
        Task<ICollection<Avatar>> Find();
        Task<Avatar?> Get(int id);
        Task<Avatar> Create(Avatar avatar);
        Task<Avatar?> Update(int id, Avatar updated);
        Task<bool> Delete(int id);
    }
}
