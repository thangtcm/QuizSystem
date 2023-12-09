using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EduQuiz_5P.Enums
{
    public enum Gender
    {
        [Display(Name = "Nam")]
        Male = 0,
        [Display(Name = "Nữ")]
        Female = 1,
        [Display(Name = "Khác")]
        Another = 2
    }
}
