using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;


namespace QuizMaster.Services
{
    public class BadgeService
    {
        private readonly QuizMasterDbContext _context;

        public BadgeService(QuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Badge>> Find()
        {
            return await _context.Badges
                .Include(b => b.UserBadges)
                .ToListAsync();
        }

        public async Task<Badge?> Get(int id)
        {
            return await _context.Badges.FindAsync(id);
        }

        public async Task<Badge> Create(Badge entity)
        {
            _context.Badges.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Badge?> Update(int id, Badge updated)
        {
            var entity = await _context.Badges.FindAsync(id);
            if (entity == null) return null;

            entity.Name = updated.Name;
            entity.Description = updated.Description;
            entity.ImageUrl = updated.ImageUrl;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Badges.FindAsync(id);
            if (entity == null) return false;

            _context.Badges.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
