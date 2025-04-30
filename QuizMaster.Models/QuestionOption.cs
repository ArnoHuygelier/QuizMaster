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
		public string OptionText { get; set; } = null!;

		[ForeignKey("QuestionId")]
		[InverseProperty("QuestionOptions")]
		public virtual Question Question { get; set; } = null!;
	}

}