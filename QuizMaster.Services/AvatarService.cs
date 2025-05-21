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
	public class AvatarService
	{
		private readonly QuizMasterDbContext _context;

		public AvatarService(QuizMasterDbContext context)
		{
			_context = context;
		}

		public async Task<ICollection<Avatar>> Find()
		{
			return await _context.Avatars.Include(c => c.Users).ToListAsync();
		}

		public async Task<Avatar?> Get(int id)
		{
			return await _context.Avatars.FindAsync(id);
		}

		public async Task<Avatar> Create(Avatar avatar)
		{
			if (avatar == null)
			{
				throw new ArgumentNullException(nameof(avatar), "Avatar cannot be null.");
			}

			await _context.Avatars.AddAsync(avatar);
			await _context.SaveChangesAsync();

			return avatar;
		}

		public async Task<Avatar?> Update(int id, Avatar updated)
		{
			var entity = await _context.Avatars.FindAsync(id);
			if (entity == null) return null;

			entity.Name = updated.Name;
			entity.AvatarUrl = updated.AvatarUrl;

			await _context.SaveChangesAsync();

			return entity;
		}

		public async Task<bool> Delete(int id)
		{
			var entity = await _context.Avatars.FindAsync(id);
			if (entity == null) return false;

			_context.Avatars.Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
