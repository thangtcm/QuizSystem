using System.ComponentModel.DataAnnotations;

namespace EduQuiz_5P.Enums
{
    public enum Subject
    {
        [Display(Name = "Toán")]
        Mathematics = 1,

        [Display(Name = "Vật Lý")]
        Physics,

        [Display(Name = "Hóa Học")]
        Chemistry,

        [Display(Name = "Sinh Học")]
        Biology,

        [Display(Name = "Ngữ Văn")]
        Literature,

        [Display(Name = "Tiếng Anh")]
        English,

        [Display(Name = "Lịch Sử")]
        History,

        [Display(Name = "Địa Lý")]
        Geography,

        [Display(Name = "Giáo Dục Công Dân")]
        CivicEducation,

        [Display(Name = "Giáo Dục Quốc Phòng - An Ninh")]
        NationalDefenseEducation,

    }
}
