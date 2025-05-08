    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace QuizMaster.Models
    {
        public class User
        {
            [Key]
            public int Id { get; set; }
            public bool IsActive { get; set; }
            public bool NewsLetter { get; set; }
            public int? AvatarId { get; set; }
            public Avatar? Avatar { get; set; }
            public int? Score { get; set; }

            public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
            public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
        }
    }
