using EduQuiz_5P.Data;
using EduQuiz_5P.Repository.GenericRepository;
using Microsoft.AspNetCore.Identity;

namespace EduQuiz_5P.Repository.Interface
{
    public interface IUserRoleRepository : IGenericRepository<IdentityUserRole<long>>
    {
    }
}
