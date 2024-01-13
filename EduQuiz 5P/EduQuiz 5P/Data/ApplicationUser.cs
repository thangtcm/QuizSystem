using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;
using EduQuiz_5P.Enums;

namespace EduQuiz_5P.Data
{
    public class ApplicationUser : IdentityUser<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        [DisplayName("Họ và tên")]
        public string? FullName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public string? ImgAvatar { get; set; }
        public double Point { get; set; }
        public ApplicationUser() { }

    }
}
