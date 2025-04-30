using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class User
	{
		[Key]
		public int UserId { get; set; }

		[StringLength(200)]
		public required string UserName { get; set; } 

		[StringLength(200)]
		public required string Email { get; set; } 

		[StringLength(50)]
		public required string Password { get; set; }

		[StringLength(50)]
		public required string Role { get; set; }

		public DateOnly CreatedAt { get; set; }

		public DateOnly LastLogin { get; set; }

		public bool IsActive { get; set; }

		public bool NewsLetter { get; set; }

		public int AvatarId { get; set; }

		[ForeignKey("AvatarId")]
		[InverseProperty("Users")]
		public required Avatar Avatar { get; set; } 

		[InverseProperty("User")]
		public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();

		[InverseProperty("CreatedByNavigation")]
		public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

		[InverseProperty("User")]
		public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();

		[InverseProperty("User")]
		public virtual ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();

		[InverseProperty("User")]
		public virtual ICollection<UserScore> UserScores { get; set; } = new List<UserScore>();
	}
}