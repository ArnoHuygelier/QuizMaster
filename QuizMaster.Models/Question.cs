using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class Question
	{
		[Key]
		public int QuestionId { get; set; }

		public int QuizId { get; set; }

		public required string QuestionText { get; set; } 

		public bool CorrectOption { get; set; }

		public int Difficulty { get; set; }

		[InverseProperty("Question")]
		public virtual ICollection<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();

		[ForeignKey("QuizId")]
		[InverseProperty("Questions")]
		public required Quiz Quiz { get; set; } 
	}
}