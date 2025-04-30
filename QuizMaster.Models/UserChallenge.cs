using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class UserChallenge
	{
		[Key]
		public int UserChallengeId { get; set; }

		public int UserId { get; set; }

		[Column("ChallengeID")]
		public int ChallengeId { get; set; }

		public DateTime CompletedAt { get; set; }

		public int RewardPoints { get; set; }

		[ForeignKey("ChallengeId")]
		[InverseProperty("UserChallenges")]
		public required DailyChallenge Challenge { get; set; }

		[ForeignKey("UserId")]
		[InverseProperty("UserChallenges")]
		public required User User { get; set; } 
	}

}