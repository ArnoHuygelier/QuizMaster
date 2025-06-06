using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface ILeaderboardService
    {
        ICollection<User> Find();
    }
}
