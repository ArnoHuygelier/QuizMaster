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
	public class UserBadgeService : IUserBadgeService
    {
		private readonly QuizMasterDbContext _context;

		public UserBadgeService(QuizMasterDbContext context)
		{
			_context = context;
		}

		public async Task<ICollection<UserBadge>> Find()
		{
			return await _context.UserBadges.Include(u => u.User).Include(b => b.Badge).ToListAsync();
		}

		public async Task<ICollection<UserBadge>> GetUserBadgesByUserId(string userId)
		{
			return await _context.UserBadges
				.Include(u => u.User)
				.Include(b => b.Badge)
				.Where(ub => ub.UserId == userId)
				.ToListAsync();
		}

		public async Task<UserBadge?> Get(int id)
		{
			return await _context.UserBadges
				.Include(u => u.User)
				.Include(b => b.Badge)
				.FirstOrDefaultAsync(ub => ub.Id == id);
		}

		public async Task<UserBadge> Create(UserBadge entity)
		{
			_context.UserBadges.Add(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<UserBadge?> Update(int id, UserBadge updated)
		{
			var entity = await _context.UserBadges.FindAsync(id);
			if (entity == null) return null;

			entity.BadgeId = updated.BadgeId;
			entity.UserId = updated.UserId;
			entity.AwardedAt = updated.AwardedAt;

			await _context.SaveChangesAsync();

			return entity;
		}

		public async Task<bool> Delete(int id)
		{
			var entity = await _context.UserBadges.FindAsync(id);
			if (entity == null) return false;

			_context.UserBadges.Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
