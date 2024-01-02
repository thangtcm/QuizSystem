using EduQuiz_5P.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.Models
{
    public class Chapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Tên Chương")]
        public string? ChapterName { get; set; }
        [Display(Name = "Mô tả Chương")]
        public string? ChapterDescription { get; set; }
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
        [Display(Name = "Người xóa")]
        public long? UserIdRemove { get; set; }
        [ForeignKey("UserIdRemove")]
        [Display(Name = "Người xóa")]
        public ApplicationUser? UserRemove { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
        [NotMapped] 
        public SelectList SelectClass { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }
    }
}
