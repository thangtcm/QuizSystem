using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Security.Claims;

namespace EduQuiz_5P.Services
{
    public class ClassService : IClassService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public ClassService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task Add(Classes @class, long userId)
        {
            @class.UserIdUpdate = userId;
            @class.DateUpdate = DateTime.Now;
            _unitOfWork.ClassRepository.Add(@class);
            await _unitOfWork.CommitAsync();
        }
        public async Task AddRange(ICollection<Classes> model, long userId)
        {
            foreach(var item in model)
            {
                item.UserIdUpdate = userId;
                item.DateUpdate = DateTime.UtcNow.ToTimeZone();
            }    
            _unitOfWork.ClassRepository.AddRange(model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> Delete(int Id)
        {
            var @class = await  _unitOfWork.ClassRepository.GetAsync(x => x.Id == Id);
            if (@class == null) return false;
            var user = await _userService.GetUser();
            @class.IsRemoved= true;
            @class.UserRemove= user;
            _unitOfWork.ClassRepository.Update(@class);
            await _unitOfWork.CommitAsync();
            return true;
        }
        public Classes? GetById(int id)
            => _unitOfWork.ClassRepository.Get(x => x.Id == id && x.IsRemoved == false);
        public async Task<Classes?> GetByIdAsync(int? id)
            => await _unitOfWork.ClassRepository.GetAsync(x => x.Id == id && x.IsRemoved == false);

        public async Task<Classes?> GetByIdAsync(int? id, Func<IQueryable<Classes>, IIncludableQueryable<Classes, object>> includes)
            => await _unitOfWork.ClassRepository.GetAsync(x => x.Id == id && x.IsRemoved == false, includes);

        public async Task<ICollection<Classes>> GetListAsync()
            => await _unitOfWork.ClassRepository.GetAllAsync();

        public async Task<ICollection<Classes>> GetListAsyncWithIncludes(Func<IQueryable<Classes>, IIncludableQueryable<Classes, object>> includes, int? Id = null, string? Name = null)
        {
            Expression<Func<Classes, bool>>? expression = null;
            if (Id.HasValue && !string.IsNullOrEmpty(Name))
            {
                expression = x => x.Id == Id.Value && x.ClassName!.Contains(Name);
            }
            else if(!string.IsNullOrEmpty(Name))
                expression = x => x.ClassName!.Contains(Name);
            else if(Id.HasValue)
                expression = x => x.Id == Id;
            return await _unitOfWork.ClassRepository.GetAllAsync(expression, includes);
        }

        public async Task Update(Classes @class)
        {
            var model = await _unitOfWork.ClassRepository.GetAsync(x => x.Id == @class.Id);
            if (model != null)
            {
                var user = await _userService.GetUser();
                model.UserUpdate = user;
                model.DateUpdate = DateTime.Now;
                model.ClassName = @class.ClassName;
                _unitOfWork.ClassRepository.Update(model);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
