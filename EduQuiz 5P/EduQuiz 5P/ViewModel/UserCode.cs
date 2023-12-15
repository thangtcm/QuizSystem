namespace EduQuiz_5P.ViewModel
{
    public class UserCode
    {
        public long UserId { get; set; }
        public UserInfoVM User { get; set; }
        public int Code { get; set; }
        public DateTime DateSend { get; set; }
    }
}
