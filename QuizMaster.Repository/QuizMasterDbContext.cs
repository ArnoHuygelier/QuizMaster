using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;

namespace QuizMaster.Repository
{
	public class QuizMasterDbContext : IdentityDbContext<User>
	{
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }


        public QuizMasterDbContext() { }

		public QuizMasterDbContext(DbContextOptions<QuizMasterDbContext> options) : base(options)
		{

		}
	}
}
