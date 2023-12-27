using EduQuiz_5P.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Tên Môn Học")]
        public string? SubjectName { get; set; }
        [Display(Name = "Mô tả Môn Học")]
        public string? SubjectDescription { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsRemoved { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdate { get; set; }
        [Display(Name = "Người cập nhật")]
        public long? UserIdUpdate { get; set; }
        [ForeignKey("UserIdUpdate")]
        [Display(Name = "Ngày cập nhật")]
        public ApplicationUser? UserUpdate { get; set; }
        [Display(Name = "Ngày xóa")]
        public DateTime DateRemove { get; set; }
        public long? UserIdRemove { get; set; }
        [ForeignKey("UserIdRemove")]
        public ApplicationUser? UserRemove { get; set; }
        public int ClassesId { get; set; }
        [ForeignKey("ClassesId")]
        public Classes? Classes { get; set; }
        [NotMapped]
        public virtual SelectList SelectClass { get; set; }
        public virtual ICollection<Chapter>? Chapters { get; set; }

        public Subject() { }
        public Subject(Subject model)
        {
            this.SubjectName = model.SubjectName;
            this.SubjectDescription = model.SubjectDescription;
            this.ClassesId = model.ClassesId;
        }
    }
}
