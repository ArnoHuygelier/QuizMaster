using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Table(nameof(Question))]
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public required string QuestionText { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}

