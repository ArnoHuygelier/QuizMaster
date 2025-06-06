using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaster.Models.Enums;

namespace QuizMaster.Models
{
    [Table(nameof(Badge))]
    public class Badge
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required string ImageUrl { get; set; }

        public BadgeType Type { get; set; }
        public int Threshold { get; set; }
        public  ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
	}

}
