using EduQuiz_5P.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EduQuiz_5P.Models;
using EduQuiz_5P.Enums;

namespace EduQuiz_5P.ViewModel
{
    public class ExamInfoVM
    {
        [Display(Name = "Mã đề thi")]
        public int ExamId { get; set; }
        [Display(Name = "Tên đề thi")]
        public string? ExamName { get; set; }
        [Display(Name = "Mô tả đề thi")]
        public string? ExamDescription { get; set; }
        [Display(Name = "Số lượng câu hỏi")]
        public int NumberOfQuestion { get; set; }
        [Display(Name = "Thời gian thi")]
        public int ExamTime { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsRemoved { get; set; }
        [Display(Name = "Ngày tạo")]
        public string DateCreate { get; set; }
        [Display(Name = "Ngày xóa")]
        public string DateRemove { get; set; }
        public bool IsDefault { get; set; }
        public string SubjectName { get; set; }
        public ExamType ExamType { get; set; }
        public int TotalUserExam { get; set; }
        public double KnowQuestion { get; set; }
        public double UnderstandQuestion { get; set; }
        public double ManipulateQuestion { get; set; }
        public ExamInfoVM()
        {

        }

        public ExamInfoVM(Exam model)
        {
            ExamId = model.Id;
            ExamName = model.ExamName;
            ExamDescription = model.ExamDescription;
            NumberOfQuestion = model.NumberOfQuestion;
            ExamTime = model.ExamTime;
            IsRemoved = model.IsRemoved;
            KnowQuestion = model.KnowQuestion;
            UnderstandQuestion = model.UnderstandQuestion;
            ManipulateQuestion = model.ManipulateQuestion;
            this.ExamType = model.ExamType;
            SubjectName = model.Subject is null ? "" : model.Subject.SubjectName ?? "";
            IsDefault = model.IsDefault;
            TotalUserExam = model.TotalUserExam;
        }
    }
}
