using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class Quiz
	{
		[Key]
		public int QuizId { get; set; }

		[StringLength(200)]
		public required string Title { get; set; } 

		public required string Description { get; set; } 

		public int CreatedBy { get; set; }

		public DateOnly CreatedAt { get; set; }

		[StringLength(50)]
		public required string Mode { get; set; } 

		public bool IsFinished { get; set; }

		[ForeignKey("CreatedBy")]
		[InverseProperty("Quizzes")]
		public virtual User CreatedByNavigation { get; set; } = null!;

		[InverseProperty("Quiz")]
		public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();

		[InverseProperty("Quiz")]
		public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

		[InverseProperty("Quiz")]
		public virtual ICollection<UserScore> UserScores { get; set; } = new List<UserScore>();
	}

}