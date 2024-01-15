namespace EduQuiz_5P.ViewModel
{
    public class StatisticalVM
    {
        public int NumberOfUser { get; set; }
        public int NumberOfExam { get; set; }
        public int NumberOfQuestion { get; set; }
        public int NumberOfUserExam { get; set; }
        public ICollection<MonthlyRegistrationData> UserRegisterData { get; set; }
        public ICollection<UserInfoVM> UserTopRank { get; set; }
    }
}
