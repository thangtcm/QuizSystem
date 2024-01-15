using EduQuiz_5P.Data;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.GenericRepository;
using EduQuiz_5P.ViewModel;

namespace EduQuiz_5P.Repository.Interface
{
    public interface IUserRegistrationRepository : IGenericRepository<UserRegistration>
    {
        Task<ICollection<MonthlyRegistrationData>> GetDailyRegistrationDataAsync();
    }
}
