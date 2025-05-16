using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Table(nameof(QuizResult))]
    public class QuizResult
    {
            [Key]
            public int Id { get; set; }

            public string UserId { get; set; }
            public User User { get; set; }

            public int QuizId { get; set; }
            public Quiz Quiz { get; set; }

            public int Score { get; set; }
            public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        }

    }
