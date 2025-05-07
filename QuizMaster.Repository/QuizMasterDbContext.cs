using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;

namespace QuizMaster.Repository
{
	public class QuizMasterDbContext(DbContextOptions<QuizMasterDbContext> options) : DbContext(options)
	{
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
    }
}
