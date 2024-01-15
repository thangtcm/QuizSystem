using EduQuiz_5P.Data;
using EduQuiz_5P.Repository.GenericRepository;
using EduQuiz_5P.Repository.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EduQuiz_5P.Repository
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<ICollection<UserInfoVM>> GetTopRank()
        {
            var model = await _dbContext.Users
                .OrderByDescending(x => x.Point)
                .Take(5)
                .AsNoTracking()
                .Select(x => new UserInfoVM
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Gender = x.Gender,
                    Point = x.Point
                })
                .ToListAsync();
            return model;
        }    
    }
}
