using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuiz_5P.Models
{
    public class UserExamDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserExamId { get; set; }
        [ForeignKey(nameof(UserExamId))]
        public UserExams UserExam { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }
        public int? SelectAnswerId { get; set; }
        [ForeignKey(nameof(SelectAnswerId))]
        public Answer? Answer { get; set; }
    }
}
