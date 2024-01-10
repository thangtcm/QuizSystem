using System.ComponentModel.DataAnnotations;

namespace EduQuiz_5P.Enums
{
    public enum ExamType
    {
        [Display(Name = "Trung học phổ thông")]
        TrungHocPhoThong = 1,        // Thông hiểu
        [Display(Name = "Lớp học")]
        Lop = 2,
    }
}
