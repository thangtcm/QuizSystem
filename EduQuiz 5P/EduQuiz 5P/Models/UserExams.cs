using EduQuiz_5P.Data;
using EduQuiz_5P.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuiz_5P.Models
{
    public class UserExams
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ExamName { get; set; }
        public int NumberOfCorrect { get; set; }
        public int ExamTime { get; set; }
        public int NumberOfQuestion { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? ApplicationUser { get; set; }
        public double Identification { get; set; }
        public double Understanding { get; set; }
        public double Application { get; set; }
        public double AdvancedApplication { get; set; }
        public ICollection<UserExamDetail>? UserExamDetails { get; set; }
        public void CreateUserExam(Exam model)
        {
            ExamName = model.ExamName ?? "";
            ExamTime = model.ExamTime;
            NumberOfQuestion = model.NumberOfQuestion;
            StartTime = DateTime.UtcNow.ToTimeZone();
            EndTime = DateTime.UtcNow.ToTimeZone().AddMinutes(model.ExamTime + 5);
            Identification = model.Identification;
            Understanding = model.Understanding;
            Application = model.Application;
            AdvancedApplication = model.AdvancedApplication;
            NumberOfCorrect = 0;
        }
    }
}
