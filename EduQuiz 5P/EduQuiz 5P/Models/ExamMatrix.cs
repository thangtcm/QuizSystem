using EduQuiz_5P.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EduQuiz_5P.Models
{
    public class ExamMatrix
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Tên Ma Trận")]
        public string? NameMatrix { get; set; }
        [DisplayName("Mô Tả Ma Trận")]
        public string? DescriptionMatrix { get; set; }
        [Display(Name = "Số lượng câu hỏi")]
        public int Total { get; set; }
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
        public bool IsDefault { get; set; }
        public ApplicationUser? UserRemove { get; set; }
        public virtual List<ExamMatrixDetail>? ExamMatrixDetail { get; set; }
    }
}
