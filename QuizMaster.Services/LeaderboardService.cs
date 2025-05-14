using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services
{
    public class LeaderboardService
    {
        private readonly QuizMasterDbContext _dbContext;

        public LeaderboardService(QuizMasterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<User> Find()
        {

            var users = _dbContext.Users
                .OrderByDescending(x => x.Score)
                .Include(x => x.Avatar)
                .Include(x => x.UserBadges)
                    .ThenInclude(y => y.Badge)
                .ToList();

            return users;
        }
    }
}