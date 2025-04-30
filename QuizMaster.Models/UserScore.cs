using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class UserScore
	{
		[Key]
		public int UserScoresId { get; set; }

		public int UserId { get; set; }

		public int QuizId { get; set; }

		public int Score { get; set; }

		public DateOnly CompletedAt { get; set; }

		public TimeOnly TimeTaken { get; set; }

		[ForeignKey("QuizId")]
		[InverseProperty("UserScores")]
		public virtual Quiz Quiz { get; set; } = null!;

		[ForeignKey("UserId")]
		[InverseProperty("UserScores")]
		public virtual User User { get; set; } = null!;
	}
}
