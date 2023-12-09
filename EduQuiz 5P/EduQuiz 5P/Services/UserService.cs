using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Net.WebSockets;
using EduQuiz_5P.Data;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;

namespace EduQuiz_5P.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser?> GetUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            return user;
        }

        public async Task<ICollection<ApplicationUser>> GetUsers()
            => await _unitOfWork.UserRepository.GetAllAsync();

        public async Task<ResponseListVM<UserInfoVM>> GetUsersWithRoles(int page = 1)
        {
           
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userroles = await _unitOfWork.UserRoleRepository.GetAllAsync();
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            int pagesize = 10;
            int totalUsers = users.Count;
            int maxpage = (totalUsers / pagesize) + (totalUsers % 10 == 0 ? 0 : 1);
            int pagenumber = page < 0 ? 1 : page;
            var userWithRoles = new List<UserInfoVM>();
            foreach (var user in users)
            {
                var userRoles = userroles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
                var matchingRoles = roles.Where(r => userRoles.Contains(r.Id)).Select(r => r.Name).ToList();
                userWithRoles.Add(new UserInfoVM(user, matchingRoles));
            }
            var data = new ResponseListVM<UserInfoVM>()
            {
                Data = userWithRoles.ToList(),
                MaxPage = maxpage
            };
            return data;
        }
        public async Task<ICollection<UserInfoVM>> GetUsersWithRoles()
        {

            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userroles = await _unitOfWork.UserRoleRepository.GetAllAsync();
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            var userWithRoles = new List<UserInfoVM>();
            foreach (var user in users)
            {
                var userRoles = userroles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
                var matchingRoles = roles.Where(r => userRoles.Contains(r.Id)).Select(r => r.Name).ToList();
                userWithRoles.Add(new UserInfoVM(user, matchingRoles));
            }
            return userWithRoles.ToList();
        }
        public async Task<ApplicationUser?> GetUser(long userId)
            => await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);

        public async Task<bool> UpdateUser(UserInfoVM user)
        {
            if (user.UserId < 1000000000) return false;
            var userModel = await _unitOfWork.UserRepository.GetAsync(x => x.Id == user.UserId!);
            if (userModel is null) return false;
            userModel.FullName = user.FullName;
            userModel.Gender = user.Gender ?? Gender.Another;
            _unitOfWork.UserRepository.Update(userModel);
            await _unitOfWork.CommitAsync();
            return true;
        }

    }
}
