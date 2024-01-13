using EduQuiz_5P.Data;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.GenericRepository;
using EduQuiz_5P.Repository.Interface;

namespace EduQuiz_5P.Repository
{
    public class UserExamRepository : GenericRepository<UserExams>, IUserExamRepository
    {
        public UserExamRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
