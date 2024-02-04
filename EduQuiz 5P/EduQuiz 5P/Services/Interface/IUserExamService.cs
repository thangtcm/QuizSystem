using EduQuiz_5P.Models;
using EduQuiz_5P.ViewModel;
using Microsoft.EntityFrameworkCore.Query;

namespace EduQuiz_5P.Services.Interface
{
    public interface IUserExamService
    {
        public Task<ICollection<UserExams>> GetListAsync(long? userId = null, Func<IQueryable<UserExams>, IIncludableQueryable<UserExams, object>>? includes = null, DateTime? date = null, bool IsOrder = false);
        public Task<UserExamInfoVM?> Add(int examId, long? userId);
        public Task<ResponResultData<UserExamInfoVM>> GenerateExamMatrix(UserExamGenerate model, long userId);
        public Task<int?> TakeAgain(int userExamId, long userId);
        public Task<int> CountAsync();
        public Task<UserExams?> GetByIdAsync(int Id, long? userId, Func<IQueryable<UserExams>, IIncludableQueryable<UserExams, object>>? includes = null);
        public UserExams? GetById(int Id);
        public Task<bool> Delete(int Id);
        public Task<int> CountCorrectAnswers(int Id);
        public Task<UserExamInfoVM?> Submit(int Id);
        public Task<int> CountQuestionComplete(int Id);
    }
}
