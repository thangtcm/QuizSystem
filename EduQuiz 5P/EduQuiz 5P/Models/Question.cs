using EduQuiz_5P.Data;
using EduQuiz_5P.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuiz_5P.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nội dung câu hỏi")]
        public string? QuestionName { get; set; }
        [Required]
        [Display(Name = "Độ khó")]
        public int LevelType { get; set; }
        [Required]
        [Display(Name = "Lời giải")]
        public string? QuestionSolution { get; set; }
        [Display(Name = "Gợi ý")]
        public string? QuestionHints { get; set; }
        [Display(Name = "Số lần thi")]
        public int NumberTimes { get; set; }
        [Display(Name = "Số lần trả lời đúng")]
        public int NumberCorrect { get; set; }
        [Display(Name = "Trạng thái xóa")]
        public bool IsRemoved { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdate { get; set; }
        [Display(Name = "Người cập nhật")]
        public long? UserIdUpdate { get; set; }
        [ForeignKey("UserIdUpdate")]
        public ApplicationUser? UserUpdate { get; set; }
        [Display(Name = "Ngày xóa")]
        public DateTime DateRemove { get; set; }
        [Display(Name = "Người xóa")]
        public long? UserIdRemove { get; set; }
        [ForeignKey("UserIdRemove")]
        public ApplicationUser? UserRemove { get; set; }
        [Display(Name = "Chương")]
        public int ChappterId { get; set; }
        [ForeignKey("ChappterId")]
        public Chapter? Chappter { get; set; }
        public string? IsImage { get; set; }
        public string? IsImageSolution { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public virtual ICollection<Answer>? Answers { get; set; }
    }
}
