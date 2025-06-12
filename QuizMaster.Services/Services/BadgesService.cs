using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Models.Enums;
using QuizMaster.Repository;
using QuizMaster.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Services.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly QuizMasterDbContext _context;
        private readonly EndGameService _endGameService;

        public BadgeService(QuizMasterDbContext context, EndGameService endGameService)
        {
            _context = context;
            _endGameService = endGameService;
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

            
            var allBadges = await _context.Badges.ToListAsync();

            
            var (quizCount, flawlessCount) = await _endGameService.GetQuizStatsAsync(userId);

            
            var scoreCount = await _context.QuizResults
                .Where(q => q.UserId == userId)
                .SumAsync(q => q.CorrectCount);


            //Loop through all badges and check if the user qualifies for any new ones
            foreach (var badge in allBadges)
            {
                if (existingBadgeIds.Contains(badge.Id))
                    continue;

                int stat;
                switch (badge.Type)
                {
                    case BadgeType.Quiz:
                        stat = quizCount;
                        break;
                    case BadgeType.Flawless:
                        stat = flawlessCount;
                        break;
                    case BadgeType.Score:
                        stat = scoreCount;
                        break;
                    default:
                        stat = 0;
                        break;
                }

                if (stat >= badge.Threshold)
                    earned.Add(badge.Id);
            }

            return earned;
        }
    }
}