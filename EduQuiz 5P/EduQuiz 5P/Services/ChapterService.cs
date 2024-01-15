using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Security.Claims;

namespace EduQuiz_5P.Services
{
    public class ChapterService : IChapterService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public ChapterService(IUnitOfWork unitOfWork, IUserService userService, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task Add(Chapter chapter)
        {
            var user = await _userService.GetUser();
            if(chapter.UploadImg != null)
            {
                chapter.UrlBackground = (await _firebaseStorageService.UploadFile(chapter.UploadImg)).ToString();
            }    
            chapter.UserUpdate = user;
            chapter.DateUpdate = DateTime.Now;
            _unitOfWork.ChapterRepository.Add(chapter);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddRange(ICollection<Chapter> chapters, long userId)
        {
            foreach(var item in chapters)
            {
                item.UserIdUpdate = userId;
                item.DateUpdate = DateTime.UtcNow.ToTimeZone();
            }    
            _unitOfWork.ChapterRepository.AddRange(chapters);
            await _unitOfWork.CommitAsync();
        }    

        public async Task<bool> Delete(int Id)
        {
            var chappter = await _unitOfWork.ChapterRepository.GetAsync(x => x.Id == Id);
            if (chappter == null) return false;
            var user = await _userService.GetUser();
            chappter.IsRemoved = true;
            chappter.UserRemove = user;
            _unitOfWork.ChapterRepository.Update(chappter);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Chapter? GetById(int? id)
            => _unitOfWork.ChapterRepository.Get(x => x.Id == id && x.IsRemoved == false);

        public async Task<Chapter?> GetByIdAsync(int? id)
            => await _unitOfWork.ChapterRepository.GetAsync(x => x.Id == id && x.IsRemoved == false);

        public async Task<Chapter?> GetByIdAsync(int? id, Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>> includes)
            => await _unitOfWork.ChapterRepository.GetAsync(x => x.Id == id && x.IsRemoved == false, includes);

        public async Task<ICollection<Chapter>> GetListAsync()
            => await _unitOfWork.ChapterRepository.GetAllAsync();

        public async Task<ICollection<Chapter>> GetListAsync(int? classId = null, int? subjectId = null, Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>>? includes = null)
        {
            if(classId.HasValue &&  subjectId.HasValue)
            {
                return await _unitOfWork.ChapterRepository.GetAllAsync(x => x.SubjectId == subjectId.Value && x.ClassesId == classId, includes);
            }
            else if(classId.HasValue && !subjectId.HasValue)
            {
                return await _unitOfWork.ChapterRepository.GetAllAsync(x => x.ClassesId == classId.Value, includes);
            }
            return await _unitOfWork.ChapterRepository.GetAllAsync(null, includes);
        }

        public async Task<ICollection<Chapter>> GetListAsyncWithIncludes(Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>> includes)
            => await _unitOfWork.ChapterRepository.GetAllAsync(null, includes);

        public async Task Update(Chapter chapter)
        {
            //var model = await _unitOfWork.ChapterRepository.GetAsync(x => x.Id == chappter.Id);
            //if (model != null)
            //{
            //    var user = await _userService.GetUser();
            //    model.UserIdUpdate = user!.Id;
            //    model.DateUpdate = DateTime.Now;
            //    model.ChapterName = chappter.ChapterName;
            //    model.ChapterDescription = chappter.ChapterDescription;

            //}
            if (chapter.UploadImg != null)
            {
                chapter.UrlBackground = (await _firebaseStorageService.UploadFile(chapter.UploadImg)).ToString();
            }
            _unitOfWork.ChapterRepository.Update(chapter);
            await _unitOfWork.CommitAsync();
        }

        public async Task SaveChanges()
            => await _unitOfWork.CommitAsync();

        public Chapter? Find(int? id)
        {
            if(id.HasValue) return _unitOfWork.ChapterRepository.Find(id);
            return null;
        }
        public async Task<Chapter?> FindAsync(int? id)
        {
            if (id.HasValue) return await _unitOfWork.ChapterRepository.FindAsync(id);
            return null;
        }
    }
}
