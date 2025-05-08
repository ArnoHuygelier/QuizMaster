using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class UserBadge
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; }

        public int BadgeId { get; set; }
        public required Badge Badge { get; set; }

        public required DateTime AwardedAt { get; set; }
    }

}
