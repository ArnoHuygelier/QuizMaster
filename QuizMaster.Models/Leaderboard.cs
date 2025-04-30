using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class Leaderboard
	{
		[Key]
		public int LeaderboardId { get; set; }

		public int QuizId { get; set; }

		public int UserId { get; set; }

		public int Score { get; set; }

		public int Rank { get; set; }

		[ForeignKey("QuizId")]
		[InverseProperty("Leaderboards")]
		public virtual Quiz Quiz { get; set; } = null!;

		[ForeignKey("UserId")]
		[InverseProperty("Leaderboards")]
		public virtual User User { get; set; } = null!;
	}

}