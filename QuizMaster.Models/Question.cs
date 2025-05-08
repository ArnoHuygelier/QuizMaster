    using System;
    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace QuizMaster.Models
    {
        public class Question
        {
            [Key]
            public int Id { get; set; }
            public string QuestionText { get; set; }

            public ICollection<Category> Categories { get; set; } = new List<Category>();
            public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
            public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        }
    }

