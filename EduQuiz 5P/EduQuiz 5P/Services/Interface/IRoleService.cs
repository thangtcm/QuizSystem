using Microsoft.AspNetCore.Identity;
using EduQuiz_5P.Data;

namespace EduQuiz_5P.Services.Interface
{
    public interface IRoleService
    {
        public Task<ICollection<ApplicationRole>> GetRoles();
        public Task<List<string>> GetUserRoles(ApplicationUser user);
    }
}
