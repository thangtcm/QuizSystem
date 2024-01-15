using EduQuiz_5P.Data;
using EduQuiz_5P.Repository.Interface;

namespace EduQuiz_5P.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IUserRoleRepository _userRoleRepository;
        private IClassRepository _classRepository;
        private IChapterRepository _chapterRepository;
        private IQuestionRepository _questionRepository;
        private ISubjectRepository _subjectRepository;
        private IAnswerRepository _answerRepository;
        private IExamRepository _examRepository;
        private IExamMatrixRepository _examMatrixRepository;
        private IExamMatrixDetailRepository _examMatrixDetailRepository;
        private IUserExamRepository _userExamRepository;
        private IUserExamDetailRepository _userExamDetailRepository;
        private IUserRegistrationRepository _userRegistrationRepository;
        private IUserActivityLogRepository _userActivityLogRepository;
        private IExamDetailRepository _examDetailRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ??= new RoleRepository(_context);
            }
        }
        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                return _userRoleRepository ??= new UserRoleRepository(_context);
            }
        }
        public IClassRepository ClassRepository
        {
            get
            {
                return _classRepository ??= new ClassRepository(_context);
            }
        }
        public IChapterRepository ChapterRepository
        {
            get
            {
                return _chapterRepository ??= new ChapterRepository(_context);
            }
        }

        public IQuestionRepository QuestionRepository
        {
            get
            {
                return _questionRepository ??= new QuestionRepository(_context);
            }
        }
        public IAnswerRepository AnswerRepository
        {
            get
            {
                return _answerRepository ??= new AnswerRepository(_context);
            }
        }
        public ISubjectRepository SubjectRepository
        {
            get
            {
                return _subjectRepository ??= new SubjectRepository(_context);
            }
        }

        public IExamMatrixDetailRepository ExamMatrixDetailRepository
        {
            get
            {
                return _examMatrixDetailRepository ??= new ExamMatrixDetailRepository(_context);
            }
        }
        public IExamMatrixRepository ExamMatrixRepository
        {
            get
            {
                return _examMatrixRepository ??= new ExamMatrixRepository(_context);
            }
        }
        public IExamRepository ExamRepository
        {
            get
            {
                return _examRepository ??= new ExamRepository(_context);
            }
        }
        public IUserExamRepository UserExamRepository
        {
            get
            {
                return _userExamRepository ??= new UserExamRepository(_context);
            }
        }

        public IUserExamDetailRepository UserExamDetailRepository
        {
            get
            {
                return _userExamDetailRepository ??= new UserExamDetailRepository(_context);
            }
        }

        public IUserRegistrationRepository UserRegistrationRepository
        {
            get
            {
                return _userRegistrationRepository ??= new UserRegistrationRepository(_context);
            }
        }
        public IUserActivityLogRepository UserActivityLogRepository
        {
            get
            {
                return _userActivityLogRepository ??= new UserActivityLogRepository(_context);
            }
        }

        public IExamDetailRepository ExamDetailRepository
        {
            get
            {
                return _examDetailRepository ??= new ExamDetailRepository(_context);
            }

        }
        public void Commit()
            => _context.SaveChanges();
        public async Task CommitAsync()
            => await _context.SaveChangesAsync();
        public void Rollback()
            => _context.Dispose();

        public async Task RollbackAsync()
            => await _context.DisposeAsync();
    }
}
