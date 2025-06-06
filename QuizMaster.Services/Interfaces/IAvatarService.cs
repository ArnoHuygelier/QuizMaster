using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
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
