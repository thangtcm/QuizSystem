using EduQuiz_5P.Enums;

namespace EduQuiz_5P.Models
{
    public class UserActivityLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public ActivityType ActivityType { get; set; } // Đăng nhập, làm bài thi, xem giải thích, v.v.
        public int? ExamId { get; set; }
        public string? ExamName { get; set; }
        public string Message { get; set; }
    }
}
