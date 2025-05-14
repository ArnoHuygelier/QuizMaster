using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Badge
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required string ImageUrl { get; set; }
		public  ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
	}

}
