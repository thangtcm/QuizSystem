using EduQuiz_5P.Models;

namespace EduQuiz_5P.ViewModel
{
    public class AnswerVMInfo
    {
        public int AnswerId { get; set; }
        public string AnswerName { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public AnswerVMInfo() { }
        public AnswerVMInfo(Answer model)
        {
            this.AnswerId = model.Id;
            this.AnswerName = model.AnswerName ?? "";
            this.QuestionId = model.QuestionId;
            this.IsCorrect =model.IsCorrect;
        }
    }
}
