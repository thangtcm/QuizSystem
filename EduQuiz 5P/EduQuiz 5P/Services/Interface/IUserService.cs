using EduQuiz_5P.Data;
using EduQuiz_5P.ViewModel;

namespace EduQuiz_5P.Services.Interface
{
    public interface IUserService
    {
        public Task<ICollection<ApplicationUser>> GetUsers();
        public Task<ResponseListVM<UserInfoVM>> GetUsersWithRoles(int page = 1);
        public Task<ICollection<UserInfoVM>> GetUsersWithRoles();
        public Task<ApplicationUser?> GetUser(long userId);
        public Task<bool> UpdateUser(UserInfoVM user);
        public Task<ApplicationUser?> GetUser();
    }
}
