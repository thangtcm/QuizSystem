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
        public ChapterService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task Add(Chapter chappter)
        {
            var user = await _userService.GetUser();
            chappter.UserUpdate = user;
            chappter.DateUpdate = DateTime.Now;
            _unitOfWork.ChapterRepository.Add(chappter);
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

        public Chapter? GetById(int id)
            => _unitOfWork.ChapterRepository.Get(x => x.Id == id && x.IsRemoved == false);

        public async Task<Chapter?> GetByIdAsync(int? id)
            => await _unitOfWork.ChapterRepository.GetAsync(x => x.Id == id && x.IsRemoved == false);

        public async Task<Chapter?> GetByIdAsync(int? id, Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>> includes)
            => await _unitOfWork.ChapterRepository.GetAsync(x => x.Id == id && x.IsRemoved == false, includes);

        public async Task<ICollection<Chapter>> GetListAsync()
            => await _unitOfWork.ChapterRepository.GetAllAsync();

        public async Task<ICollection<Chapter>> GetListAsync(int classId)
            => await _unitOfWork.ChapterRepository.GetAllAsync(x => x.Id == classId);

        public async Task<ICollection<Chapter>> GetListAsyncWithIncludes(Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>> includes)
            => await _unitOfWork.ChapterRepository.GetAllAsync(null, includes);

        public async Task Update(Chapter chappter)
        {
            var model = await _unitOfWork.ChapterRepository.GetAsync(x => x.Id == chappter.Id);
            if (model != null)
            {
                var user = await _userService.GetUser();
                model.UserIdUpdate = user!.Id;
                model.DateUpdate = DateTime.Now;
                model.ChapterName = chappter.ChapterName;
                model.ChapterDescription = chappter.ChapterDescription;
                _unitOfWork.ChapterRepository.Update(model);
                await _unitOfWork.CommitAsync();
            }
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
