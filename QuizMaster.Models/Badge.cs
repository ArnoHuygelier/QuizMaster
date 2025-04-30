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
		public string BadgeName { get; set; } = null!;

		public string Description { get; set; } = null!;

		public string Criteria { get; set; } = null!;

		[InverseProperty("Badge")]
		public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
	}

}