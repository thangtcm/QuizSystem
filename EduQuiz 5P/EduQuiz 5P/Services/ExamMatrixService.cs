using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EduQuiz_5P.Services
{
    public class ExamMatrixService : IExamMatrixService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public ExamMatrixService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<ICollection<ExamMatrix>> GetListAsync(int? subjectId = null, Func<IQueryable<ExamMatrix>, IIncludableQueryable<ExamMatrix, object>>? includes = null, string? Name = null)
            => await _unitOfWork.ExamMatrixRepository.GetAllAsync(x => x.ExamMatrixName!.Contains(Name ?? "") && (subjectId.HasValue ? x.SubjectId == subjectId.Value : true), includes);

        public async Task Add(ExamMatrix examMatrix, long userId)
        {
            examMatrix.UserIdUpdate = userId;
            examMatrix.DateUpdate = DateTime.UtcNow.ToTimeZone();
            ICollection<ExamMatrixDetail> examMatrixDetails = new List<ExamMatrixDetail>();
            if(examMatrix.ExamMatrixDetail != null)
            {
                examMatrixDetails = examMatrix.ExamMatrixDetail;
                foreach (var item in examMatrixDetails)
                {
                    item.ExamMatrix = examMatrix;
                }
                examMatrix.IsDefault = true;
                examMatrix.Total = examMatrix.ExamMatrixDetail.Sum(x => x.NumberOfQuestion);
                examMatrix.ExamMatrixDetail = null;
            }
            _unitOfWork.ExamMatrixRepository.Add(examMatrix);
            _unitOfWork.ExamMatrixDetailRepository.AddRange(examMatrixDetails);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ExamMatrix?> GetByIdAsync(int id)
            => await _unitOfWork.ExamMatrixRepository.GetAsync(x => x.Id == id && x.IsRemoved == false);

        public async Task<ExamMatrix?> GetByIdAsync(int id, Func<IQueryable<ExamMatrix>, IIncludableQueryable<ExamMatrix, object>>? includes = null)
            => await _unitOfWork.ExamMatrixRepository.GetAsync(x => x.Id == id && x.IsRemoved == false, includes);

        public ExamMatrix? GetById(int id)
            => _unitOfWork.ExamMatrixRepository.Get(x => x.Id == id && x.IsRemoved == false);

        public async Task<bool> Delete(int Id)
        {
            var examMatrix = await _unitOfWork.ExamMatrixRepository.GetAsync(x => x.Id == Id);
            if (examMatrix == null) return false;
            var user = await _userService.GetUser();
            examMatrix.IsRemoved = true;
            examMatrix.UserRemove = user;
            _unitOfWork.ExamMatrixRepository.Update(examMatrix);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task Update(ExamMatrix examMatrix)
        {
            var user = await _userService.GetUser();
            if (user != null)
            {
                examMatrix.UserUpdate = user;
            }
            _unitOfWork.ExamMatrixRepository.Update(examMatrix);
            await _unitOfWork.CommitAsync();
        }

    }
}
