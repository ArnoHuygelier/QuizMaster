using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
        public  User User { get; set; }

        public int CategoryId { get; set; }
        public  Category Category { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }

}
