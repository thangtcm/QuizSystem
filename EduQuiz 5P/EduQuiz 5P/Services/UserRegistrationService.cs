using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;

namespace EduQuiz_5P.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        public IUnitOfWork _unitOfWork;
        public UserRegistrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(long UserId)
        {
            UserRegistration model = new()
            {
                UserId = UserId,
            };
            _unitOfWork.UserRegistrationRepository.Add(model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<int> CountAsync(DateTime date)
            => await _unitOfWork.UserRegistrationRepository.CountAsync(x => x.RegistrationDate.Day == date.Day);

        public async Task<ICollection<MonthlyRegistrationData>> GetListAysnc()
        {
            return await _unitOfWork.UserRegistrationRepository.GetDailyRegistrationDataAsync();
        }
    }
}
