using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;

namespace QuizMaster.Repository
{
	public class QuizMasterDbContext(DbContextOptions<QuizMasterDbContext> options) : DbContext(options)
	{
		public DbSet<Avatar> Avatars { get; set; }
		public DbSet<Badge> Badges { get; set; }
		public DbSet<DailyChallenge> DailyChallenges { get; set; }
		public DbSet<Leaderboard> Leaderboards { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<QuestionOption> QuestionOptions { get; set; }
		public DbSet<Quiz> Quizzes { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserBadge> UserBadges { get; set; }
		public DbSet<UserChallenge> UserChallenges { get; set; }
		public DbSet<UserScore> UserScores { get; set; }
	}
}
