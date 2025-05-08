using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using QuizMaster.Services.Interfaces;

namespace QuizMaster.Services
{
    public class UserService : ICrudService<User>
    {
        private readonly QuizMasterDbContext _dbContext;

        public UserService(QuizMasterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<User>> Find()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> Get(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User?> Create(User entity)
        {
            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<User?> Update(int id, User entity)
        {
            var tempUser = await Get(id);

            if (tempUser is null)
            {
                return null;
            }
            await _dbContext.SaveChangesAsync();
            return tempUser;
        }
        public async Task Delete(int id)
        {
            var user = await Get(id);

            if (user is null)
            {
                return;
            }


            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();
        }
    }
}
