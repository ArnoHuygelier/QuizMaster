using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class DailyChallenge
	{
		[Key]
		public int ChallengeId { get; set; }

		[StringLength(200)]
		public required string ChallengeTitle { get; set; }

		public required string ChallengeDescription { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int RewardPoints { get; set; }

		[InverseProperty("Challenge")]
		public  ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
	}

}