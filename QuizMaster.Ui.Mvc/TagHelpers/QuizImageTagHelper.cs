    using Microsoft.AspNetCore.Razor.TagHelpers;

namespace QuizMaster.Ui.Mvc.TagHelpers
{

    /// <summary>
    /// 
    /// This custom TagHelper renders an img tag with the provided quiz image URL.
    /// If no image is provided (null or empty), it falls back to using `/images/quizimage/defaultQuiz.png`.
    /// 
    /// Usage example:
    /// <quiz-image image-url="@quiz.ImageUrl" class="card-img-top" alt="@quiz.Title" />
    ///
    ///
    /// Supported attributes:
    /// - image-url (string): The name of the image file (e.g., "myquiz.jpg").
    /// - alt (string): Optional alt text for accessibility. Defaults to "Quiz image".
    /// - class (string): Optional CSS classes to apply to the image.
    /// - loading (string): Optional loading behavior ("lazy", "eager", etc.). Defaults to "lazy".
    ///
    /// This TagHelper is useful for displaying consistent quiz thumbnails across pages like:
    /// - Homepage
    /// - Quiz Details page
    /// - Quiz Play page 
    /// </summary>
    
    [HtmlTargetElement("quiz-image")]
    public class QuizImageTagHelper : TagHelper
    {
        private readonly IWebHostEnvironment _env;

        public QuizImageTagHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HtmlAttributeName("image-url")]
        public string? ImageUrl { get; set; }

        [HtmlAttributeName("alt")]
        public string Alt { get; set; } = "Quiz image";

        [HtmlAttributeName("class")]
        public string? CssClass { get; set; }

        [HtmlAttributeName("loading")]
        public string Loading { get; set; } = "lazy";

        [HtmlAttributeName("width")]
        public string? Width { get; set; }

        [HtmlAttributeName("height")]
        public string? Height { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string resolvedUrl;
            if (string.IsNullOrWhiteSpace(ImageUrl))
            {
                resolvedUrl = "/images/quizimage/defaultQuiz.png";
            }
            else
            {
                var imagePath = Path.Combine(_env.WebRootPath, "images", "quizimage", ImageUrl);
                if (!File.Exists(imagePath))
                {
                    resolvedUrl = "/images/quizimage/defaultQuiz.png";
                }
                else
                {
                    resolvedUrl = $"/images/quizimage/{ImageUrl}";
                }
            }

            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            output.Attributes.SetAttribute("src", resolvedUrl);
            output.Attributes.SetAttribute("alt", Alt);
            output.Attributes.SetAttribute("loading", Loading);

            if (!string.IsNullOrWhiteSpace(CssClass))
                output.Attributes.SetAttribute("class", CssClass);

            if (!string.IsNullOrWhiteSpace(Width))
                output.Attributes.SetAttribute("width", Width);

            if (!string.IsNullOrWhiteSpace(Height))
                output.Attributes.SetAttribute("height", Height);
        }
    }
}
