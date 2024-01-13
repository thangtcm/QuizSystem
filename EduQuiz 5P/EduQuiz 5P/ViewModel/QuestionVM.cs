using EduQuiz_5P.Enums;
using EduQuiz_5P.Models;

namespace EduQuiz_5P.ViewModel
{
    public class QuestionVM
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionHints { get; set; }
        public string QuestionSolution { get; set; }
        public IFormFile? UploadImageQuestion { get; set; }
        public IFormFile? UploadImageQuestionSolution { get; set; }
        public string UrlImage { get; set; }
        public string UrlImageSolution { get; set; }
        public int? ChapterId { get; set; }
        public List<AnswerVMInfo> AnswerList { get; set; }
        public QuestionVM() { }
        public QuestionVM(Question model)
        {
            this.QuestionId = model.Id;
            this.QuestionName = model.QuestionName ?? "";
            this.QuestionSolution = model.QuestionSolution ?? "";
            this.UrlImageSolution = model.IsImageSolution ?? "";
            this.UrlImage = model.IsImage ?? "";
            this.AnswerList = model.Answers is null ? new List<AnswerVMInfo>() : model.Answers.Select(x => new AnswerVMInfo(x)).ToList();
        }
    }
}
