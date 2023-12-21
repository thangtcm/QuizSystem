using EduQuiz_5P.Repository.Interface;

namespace EduQuiz_5P.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IClassRepository ClassRepository { get; }
        IChapterRepository ChapterRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
