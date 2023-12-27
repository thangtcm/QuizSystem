using EduQuiz_5P.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EduQuiz_5P.Models
{
    public class Classes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Lớp")]
        public string? ClassName { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsRemoved { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdate { get; set; }
        [Display(Name = "Người cập nhật")]
        public long? UserIdUpdate { get; set; }
        [ForeignKey("UserIdUpdate")]
        [Display(Name = "Người cập nhật")]
        public ApplicationUser? UserUpdate { get; set; }
        [Display(Name = "Ngày xóa")]
        public DateTime DateRemove { get; set; }
        [Display(Name = "Người xóa")]
        public long? UserIdRemove { get; set; }
        [ForeignKey("UserIdRemove")]
        [Display(Name = "Người xóa")]
        public ApplicationUser? UserRemove { get; set; }
        public virtual ICollection<Subject>? Subject { get; set; }
        public Classes() { }

        public Classes(Classes model)
        {
            ClassName = model.ClassName;
            Subject = model.Subject;
        }
    }
}
