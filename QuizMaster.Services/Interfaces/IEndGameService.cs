using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public interface IEndGameService
    {
       Task<(int quizCount, int flawlessCount)> GetQuizStatsAsync(string userId);
    }
}
