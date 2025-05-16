using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Table(nameof(UserBadge))]
    public class UserBadge
    {
        [Key]
        public int Id { get; set; }

        public required string UserId { get; set; }
        public required User User { get; set; }

        public int BadgeId { get; set; }
        public required Badge Badge { get; set; }

        public required DateTime AwardedAt { get; set; }
    }

}
