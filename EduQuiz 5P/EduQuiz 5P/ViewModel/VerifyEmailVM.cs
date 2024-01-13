using EduQuiz_5P.Enums;

namespace EduQuiz_5P.ViewModel
{
    public class VerifyEmailVM
    {
        public int Code { get; set; }
        public string Email { get; set; }
        public UserInfoVM? User { get; set; }
        public VerifyEmailType EmailType { get; set; }
        public DateTime ExpiryCode { get; set; }
    }
}
