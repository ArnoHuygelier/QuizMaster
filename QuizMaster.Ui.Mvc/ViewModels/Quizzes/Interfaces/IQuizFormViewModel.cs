namespace QuizMaster.Ui.Mvc.Models.Quizzes.Interfaces
{

    public interface IQuizFormViewModel
    {
        string Title { get; set; }
        string Description { get; set; }
        int? CategoryId { get; set; }
    }

}
