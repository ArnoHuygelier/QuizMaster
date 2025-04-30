using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class Avatar
	{
		[Key]
		public int AvatarId { get; set; }

		public string AvatarUrl { get; set; } = null!;

		public DateTime UploadedAt { get; set; }

		[InverseProperty("Avatar")]
		public virtual ICollection<User> Users { get; set; } = new List<User>();
	}
}
