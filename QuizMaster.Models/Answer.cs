using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public required string AnswerText { get; set; }

        public required bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public required Question Question { get; set; }
    }


}
