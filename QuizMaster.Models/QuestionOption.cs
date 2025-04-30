using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
	public partial class QuestionOption
	{
		[Key]
		public int OptionsId { get; set; }

		public int QuestionId { get; set; }

		[StringLength(255)]
		public required string OptionText { get; set; }

		[ForeignKey("QuestionId")]
		[InverseProperty("QuestionOptions")]
		public required Question Question { get; set; } 
	}

}