using EduQuiz_5P.Models;
using EduQuiz_5P.ViewModel;

namespace EduQuiz_5P.Services.Interface
{
    public interface IUserRegistrationService
    {
        public Task<ICollection<MonthlyRegistrationData>> GetListAysnc();
        public Task<int> CountAsync(DateTime date);
        public Task Add(long UserId);
    }
}
