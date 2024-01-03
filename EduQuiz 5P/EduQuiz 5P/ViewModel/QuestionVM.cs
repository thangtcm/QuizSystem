using EduQuiz_5P.Enums;

namespace EduQuiz_5P.ViewModel
{
    public class QuestionVM
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionHints { get; set; }
        public string QuestionSolution { get; set; }
        public IFormFile UploadImageQuestion { get; set; }
        public IFormFile UploadImageQuestionSolution { get; set; }
        public string UrlImage { get; set; }
        public string UrlImageSolution { get; set; }
        public int? ChapterId { get; set; }
        public ICollection<AnswerVMInfo> AnswerList { get; set; }
    }
}
