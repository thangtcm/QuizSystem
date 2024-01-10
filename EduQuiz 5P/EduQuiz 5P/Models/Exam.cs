using EduQuiz_5P.Data;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuiz_5P.Models
{
    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã đề thi")]
        public int Id { get; set; }
        [Display(Name = "Tên đề thi")]
        public string? ExamName { get; set; }
        [Display(Name = "Mô tả đề thi")]
        public string? ExamDescription { get; set; }
        [Display(Name = "Số lượng câu hỏi")]
        public int NumberOfQuestion { get; set; }
        [Display(Name = "Thời gian thi")]
        public int ExamTime { get; set; }
        public int? ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Classes? Classes { get; set; }
        public int? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }   
        public int? ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public Chapter? Chapter { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsRemoved { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Người tạo")]
        public long? UserIdCreate { get; set; }
        [ForeignKey("UserIdCreate")]
        public ApplicationUser? UserCreate { get; set; }
        [Display(Name = "Ngày xóa")]
        public DateTime DateRemove { get; set; }
        [Display(Name = "Người xóa")]
        public long? UserIdRemove { get; set; }
        [ForeignKey("UserIdRemove")]
        public ApplicationUser? UserRemove { get; set; }
        public bool IsDefault { get; set; }
        public ExamType ExamType { get; set; }
        public int TotalUserExam { get; set; }
        public string ListQuestion { get; set; }

        public void ExamCreate(ExamInfoVM model, long UserIdControl)
        {
            ExamName = model.ExamName;
            ExamDescription = model.ExamDescription;
            NumberOfQuestion = model.NumberOfQuestion;
            ExamTime = model.ExamTime;
            IsRemoved = model.IsRemoved;
            IsDefault = model.IsDefault;
            TotalUserExam = model.TotalUserExam;
            ListQuestion = (model.ListQuestion is null || model.ListQuestion.Count == 0) ? "" : string.Join(", ", model.ListQuestion);
            UserIdCreate = UserIdControl;
            DateCreate = DateTime.UtcNow.ToTimeZone();
        }
    }
}
