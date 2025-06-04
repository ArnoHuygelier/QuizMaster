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



        /// <summary>
        /// Checks which badges a user has earned based on quiz activity and assigns any new ones.
        /// </summary>
        /// 
        /// <returns></returns>
        public async Task<List<Badge>> CheckAndAssignBadges(string userId)
        {
            // Fetch the user including their currently earned badges
            var user = await _context.Users
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Id == userId);

            // If user doesn't exist, return an empty list
            if (user == null) return new List<Badge>();

            // Get a list of badge IDs the user already has
            var userBadgeIds = user.UserBadges.Select(ub => ub.BadgeId).ToList();

            // Determine which new badges the user has earned but doesn't have yet
            var earnedBadgeIds = await CalculateEarnedBadges(userId, userBadgeIds);

            // If no new badges were earned, return an empty list
            if (earnedBadgeIds.Count == 0) return new List<Badge>();

            // Fetch full badge objects for the newly earned badge IDs
            var badges = await _context.Badges
                .Where(b => earnedBadgeIds.Contains(b.Id))
                .ToListAsync();

            // Create new UserBadge entries to represent earned badges
            var userBadges = badges.Select(badge => new UserBadge
            {
                UserId = userId,
                BadgeId = badge.Id,
                AwardedAt = DateTime.UtcNow,
                User = user,
                Badge = badge
            }).ToList();

            // Add new UserBadge records to the context
            _context.UserBadges.AddRange(userBadges);

            // Save all changes to the database
            await _context.SaveChangesAsync();

            // Return the list of newly earned badges
            return badges;
        }


        private async Task<List<int>> CalculateEarnedBadges(string userId, List<int> existingBadgeIds)
        {
            var earned = new List<int>();

            // Load all badge metadata from DB once
            var allBadges = await _context.Badges.ToListAsync();

            // Create a lookup dictionary for quick access by name
            var badgeLookup = allBadges.ToDictionary(b => b.Name.Trim().ToLower(), b => b.Id);

            // Count user quiz attempts and flawless quizzes
            var quizCount = await _context.QuizResults
                
                .CountAsync(q => q.UserId == userId);

            var flawlessCount = await _context.QuizResults
                .Include(q => q.Quiz)
                .ThenInclude(quiz => quiz.Questions)
                .Where(q => q.UserId == userId && q.Score == q.Quiz.Questions.Count)
                .CountAsync();

            // Define badge rules as (badgeName, type, threshold)
            var badgeRules = new List<BadgeRule>
            {
                new BadgeRule { BadgeName = "beginner", Type = "quiz", Threshold = 1 },
                new BadgeRule { BadgeName = "active participant", Type = "quiz", Threshold = 10 },
                new BadgeRule { BadgeName = "quiz veteran", Type = "quiz", Threshold = 50 },
                new BadgeRule { BadgeName = "flawless victory", Type = "flawless", Threshold = 1 },
                new BadgeRule { BadgeName = "perfection seeker", Type = "flawless", Threshold = 10 },
                new BadgeRule { BadgeName = "mastermind", Type = "flawless", Threshold = 50 }

            };

            foreach (var rule in badgeRules)
            {
                if (!badgeLookup.TryGetValue(rule.BadgeName, out var badgeId))
                    continue;
                if (existingBadgeIds.Contains(badgeId))
                    continue;

                int stat = rule.Type == "quiz" ? quizCount : flawlessCount;
                if (stat >= rule.Threshold)
                    earned.Add(badgeId);
            }
            

            return earned;
        }
    }
}


    

