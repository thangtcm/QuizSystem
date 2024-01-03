using EduQuiz_5P.Data;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.GenericRepository;
using EduQuiz_5P.Repository.Interface;

namespace EduQuiz_5P.Repository
{
    public class ExamMatrixRepository : GenericRepository<ExamMatrix>, IExamMatrixRepository
    {
        public ExamMatrixRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
