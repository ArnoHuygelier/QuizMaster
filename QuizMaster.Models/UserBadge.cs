using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class UserBadge
	{
		[Key]
		public int UserBadgeId { get; set; }

		public int UserId { get; set; }

		public int BadgeId { get; set; }

		public DateTime AchievedAt { get; set; }

		[ForeignKey("BadgeId")]
		[InverseProperty("UserBadges")]
		public required Badge Badge { get; set; } 

		[ForeignKey("UserId")]
		[InverseProperty("UserBadges")]
		public required User User { get; set; } 
	}

}