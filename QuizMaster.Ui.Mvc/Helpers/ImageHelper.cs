namespace QuizMaster.Ui.Mvc.Helpers
{
    public static class ImageHelper
    {
        public static string GetQuizImageUrl(string? imageUrl)
        {
            var fileName = string.IsNullOrWhiteSpace(imageUrl) ? "defaultQuiz.png" : imageUrl;
            return $"/images/quizimage/{fileName}";
        }
    }
}
