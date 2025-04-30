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
		public string ChallengeTitle { get; set; } = null!;

		public string ChallengeDescription { get; set; } = null!;

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int RewardPoints { get; set; }

		[InverseProperty("Challenge")]
		public virtual ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
	}

}