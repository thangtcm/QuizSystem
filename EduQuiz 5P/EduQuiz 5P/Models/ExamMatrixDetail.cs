using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EduQuiz_5P.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.Models
{
    public class ExamMatrixDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int ChappterId { get; set; }
        [ForeignKey("ChappterId")]
        [DisplayName("Chương")]
        public Chapter? Chapter { get; set; }
        [DisplayName("Số lượng câu hỏi")]
        public int NumberOfQuestion { get; set; }
        public int ExamMatrixId { get; set; }
        [ForeignKey("ExamMatrixId")]
        [DisplayName("Ma trận")]
        public ExamMatrix? ExamMatrix { get; set; }
        public DifficultyLevel Component { get; set; }
        [NotMapped]
        public SelectList SelectListClass { get; set; }
        [NotMapped]
        public SelectList SelectListSubject { get; set; }
    }
}
