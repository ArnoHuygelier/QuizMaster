using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Table(nameof(Category))]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        
    }

}
