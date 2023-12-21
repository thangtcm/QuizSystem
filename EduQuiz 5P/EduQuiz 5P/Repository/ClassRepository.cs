using EduQuiz_5P.Data;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.GenericRepository;
using EduQuiz_5P.Repository.Interface;

namespace EduQuiz_5P.Repository
{
    public class ClassRepository : GenericRepository<Classes>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
