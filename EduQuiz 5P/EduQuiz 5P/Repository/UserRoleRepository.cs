using EduQuiz_5P.Data;
using EduQuiz_5P.Repository.GenericRepository;
using EduQuiz_5P.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace EduQuiz_5P.Repository
{
    public class UserRoleRepository : GenericRepository<IdentityUserRole<long>>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
