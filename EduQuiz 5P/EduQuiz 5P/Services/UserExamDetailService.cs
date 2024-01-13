using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;

namespace EduQuiz_5P.Services
{
    public class UserExamDetailService : IUserExamDetailService
    {
        public IUnitOfWork _unitOfWork;
        public UserExamDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SelectAnswer(int Id, int AnswerId)
        {
            var userExamDetail = await _unitOfWork.UserExamDetailRepository.GetAsync(x => x.Id == Id);
            if(userExamDetail != null) { 
                userExamDetail.SelectAnswerId = AnswerId;
                _unitOfWork.UserExamDetailRepository.Update(userExamDetail);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
