using System.ComponentModel.DataAnnotations;

namespace EduQuiz_5P.Enums
{
    public enum DifficultyLevel
    {
        [Display(Name = "Nhận biết")]
        RECOGNITION = 1,        // Nhận biết
        [Display(Name = "Thông hiểu")]
        UNDERSTANDING = 2,        // Thông hiểu
        [Display(Name = "Vận dụng")]
        APPLICATION = 3,          // Vận dụng
        [Display(Name = "Vận dụng cao")]
        HIGHER_ORDER_APPLICATION = 4        // Vận dụng cao
    }
}
