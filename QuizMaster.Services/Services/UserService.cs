using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using QuizMaster.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuizMaster.Services.Services
{
    public class UserService : IUserService
    {
        private readonly QuizMasterDbContext _context;

        public UserService(QuizMasterDbContext context)
        {
            _context = context;
        }

        // Alles opvragen
        public async Task<IEnumerable<User>> Find()
        {
            return await _context.Users.Include(u => u.Avatar).ToListAsync();
        }

		// Eén item opvragen via ID
		public async Task<User?> Get(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Nieuw item aanmaken
        public async Task<User> Create(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Bestaand item bijwerken
        public async Task<User?> Update(string id, User updated)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity == null) return null;

            entity.UserName = updated.UserName;
            entity.Email = updated.Email;
            entity.IsActive = updated.IsActive;

            await _context.SaveChangesAsync();
            return entity;
        }

        // Verwijderen op ID
        public async Task<bool> Delete(string id)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity == null) return false;

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}