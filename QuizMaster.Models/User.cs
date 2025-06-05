using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
	[Table(nameof(User))]
	public class User : IdentityUser
	{
		public bool IsActive { get; set; }
		public bool NewsLetter { get; set; }
		public int? AvatarId { get; set; }
		public Avatar? Avatar { get; set; }
		public int Score { get; set; }

        public int Hints { get; set; } = 3;
		public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
		public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
	}
}
