using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using Microsoft.EntityFrameworkCore.Query;

namespace EduQuiz_5P.Services
{
    public class SubjectService : ISubjectService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public SubjectService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _firebaseStorageService = firebaseStorageService;
        }
        public async Task Add(Subject subject, long userId)
        {
            if(subject.UploadImage != null)
            {
                subject.UrlBackground = (await _firebaseStorageService.UploadFile(subject.UploadImage)).ToString();
            }    
            subject.UserIdUpdate = userId;
            subject.DateUpdate = DateTime.UtcNow.ToTimeZone();
            _unitOfWork.SubjectRepository.Add(subject);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddRange(ICollection<Subject> subjects, long userId)
        {
            foreach(var item in subjects)
            {
                item.UserIdUpdate = userId;
                item.DateUpdate = DateTime.UtcNow.ToTimeZone();
            }    
            _unitOfWork.SubjectRepository.AddRange(subjects);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> Delete(int Id)
        {
            var subject = await _unitOfWork.SubjectRepository.GetAsync(x => x.Id == Id);
            if (subject == null) return false;
            _unitOfWork.SubjectRepository.Remove(subject);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Subject? GetById(int? id)
            => _unitOfWork.SubjectRepository.Get(x => x.Id == id);

        public async Task<Subject?> GetByIdAsync(int? id)
            => await _unitOfWork.SubjectRepository.GetAsync(x => x.Id == id);

        public async Task<Subject?> GetByIdAsync(int? id, Func<IQueryable<Subject>, IIncludableQueryable<Subject, object>> includes)
            => await _unitOfWork.SubjectRepository.GetAsync(x => x.Id == id, includes);

        public async Task<ICollection<Subject>> GetListAsync(Func<IQueryable<Subject>, IIncludableQueryable<Subject, object>>? includes = null)
            => await _unitOfWork.SubjectRepository.GetAllAsync();

        public async Task Update(Subject subject)
        {
            var model = await _unitOfWork.SubjectRepository.GetAsync(x => x.Id == subject.Id);
            if (model != null)
            {
                model.SubjectDescription = subject.SubjectDescription;
                model.SubjectName = subject.SubjectName;
                if (subject.UploadImage != null)
                {
                    Console.WriteLine("RUnnn \n\n\n\n");
                    model.UrlBackground = (await _firebaseStorageService.UploadFile(subject.UploadImage)).ToString();
                }
                _unitOfWork.SubjectRepository.Update(model);
                await _unitOfWork.CommitAsync();
            }
           
        }
    }
}
