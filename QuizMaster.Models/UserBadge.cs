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
        public User User { get; set; }

        public int BadgeId { get; set; }
        public Badge Badge { get; set; }

        public DateTime AwardedAt { get; set; }
    }

}
