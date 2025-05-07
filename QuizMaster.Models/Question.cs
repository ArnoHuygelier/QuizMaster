    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace QuizMaster.Models
    {
        public class Question
        {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
