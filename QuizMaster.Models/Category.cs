using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }

}
