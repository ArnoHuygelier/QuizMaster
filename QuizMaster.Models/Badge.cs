using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class Badge
	{
		[Key]
		[Column("BadgeID")]
		public int BadgeId { get; set; }

		[StringLength(200)]
		public required string BadgeName { get; set; } 

		public required string Description { get; set; } 

		public required string Criteria { get; set; } 

		[InverseProperty("Badge")]
		public  ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
	}

}