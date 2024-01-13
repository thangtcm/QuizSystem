using EduQuiz_5P.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuiz_5P.ViewModel
{
    public class UserExamInfoVM
    {
        public int UserExamId { get; set; }
        public string ExamName { get; set; }
        public long? UserId { get; set; }
        public ICollection<UserExamDetailInfoVM> UserExamDetailVM { get; set; } = new List<UserExamDetailInfoVM>();
        public bool IsSession { get; set; } = true;
        public int NumberOfCorrect { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfQuestion { get; set; }
        public int QuestionComplete { get; set; }
        public UserExamInfoVM() { }
        public UserExamInfoVM(UserExams model)
        {
            ExamName = model.ExamName;
            UserExamId = model.Id;
            UserId = model.UserId;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            QuestionComplete = 0;
            NumberOfQuestion = model.NumberOfQuestion;
            UserExamDetailVM = model.UserExamDetails is null ? new List<UserExamDetailInfoVM>() : model.UserExamDetails.Select(x => new UserExamDetailInfoVM(x)).ToList();
        }    
        
    }
    public class UserExamDetailInfoVM
    {
        public int UserExamDetailId { get; set; }
        public QuestionVM QuestionVM { get; set; }
        public int? SelectAnswerId { get; set; }
        public UserExamDetailInfoVM() { }
        public UserExamDetailInfoVM(UserExamDetail model)
        {
            UserExamDetailId = model.Id;
            SelectAnswerId = model.SelectAnswerId;
            QuestionVM = model.Question is null ? new QuestionVM() : new QuestionVM(model.Question);
        }
    }
}
