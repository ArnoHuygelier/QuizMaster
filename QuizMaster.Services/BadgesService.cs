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

        public async Task CheckAndAssignBadgesAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return;

            var userBadges = user.UserBadges.Select(ub => ub.BadgeId).ToList();
            var earned = new List<int>();

            var quizCount = await _context.QuizResults
                .Where(q => q.UserId == userId)
                .CountAsync();
            var flawlessCount = await _context.QuizResults
                .Where(q => q.UserId == userId && q.Score == q.Quiz.Questions.Count)
                .Include(q => q.Quiz)
                .ThenInclude(quiz => quiz.Questions)
                .CountAsync();
            

            // Beginner badge
            if (quizCount >= 1 && !userBadges.Contains(7)) earned.Add(7); // Beginner
            if (quizCount >= 10 && !userBadges.Contains(8)) earned.Add(8); // Active Participant
            if (quizCount >= 50 && !userBadges.Contains(9)) earned.Add(9); // Quiz Veteran

            if (flawlessCount >= 1 && !userBadges.Contains(4)) earned.Add(4); // Flawless Victory
            if (flawlessCount >= 10 && !userBadges.Contains(5)) earned.Add(5); // Perfection Seeker
            if (flawlessCount >= 50 && !userBadges.Contains(6)) earned.Add(6); // Mastermind

            

            foreach (var badgeId in earned)
            {
                _context.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badgeId,
                    AwardedAt = DateTime.UtcNow,
                    User = user,
                    Badge = await Get(badgeId)
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
