namespace EduQuiz_5P.Services.Interface
{
    public interface IUserExamDetailService
    {
        public Task SelectAnswer(int Id, int AnswerId);
    }
}
