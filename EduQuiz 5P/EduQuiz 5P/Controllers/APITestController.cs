using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduQuiz_5P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APITestController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public IUnitOfWork _unitOfWork;
        public APITestController(IExamService examService, IFirebaseStorageService firebaseStorageService, IUnitOfWork unitOfWork)
        {
            _examService = examService;
            _firebaseStorageService = firebaseStorageService;
            _unitOfWork = unitOfWork;
        }

        //[HttpPost("Testupload")]
        //public async Task<IActionResult> Testupload(IFormFile upload)
        //{
        //    try
        //    {
        //        var result = await _firebaseStorageService.UploadFile(upload);
        //        return Ok(result.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return NotFound();
        //}



        //[HttpGet("RandomQuestion")]
        //public async Task<IActionResult> RandomQuestion()
        //{
        //    try
        //    {
        //        //Exam model = new()
        //        //{
        //        //    ExamName = exam.ExamName,
        //        //    ExamDescription = exam.ExamDescription,
        //        //    ExamTime = exam.ExamTime,
        //        //};
        //        //var result = await _examService.CreateExamWithMatrix(model, 2, 1);
        //        //if (result.IsSuccess)
        //        //{
        //        //    return Ok(result.Result!.Questions);
        //        //}
        //        //else
        //        //{
        //        //    return BadRequest(result.Message);
        //        //}
        //        var randomQuestions = await _unitOfWork.QuestionRepository.GetAllAsync(
        //           x => x.IsRemoved == false && x.Chappter!.ClassesId == 12 && x.Chappter!.SubjectId == 2,
        //           x => x.Include(c => c.Chappter!),
        //           query => query.OrderBy(q => Guid.NewGuid()),
        //           60);
                
        //        return Ok(randomQuestions.Select(x => new QuestionVM(x)).ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return NotFound();
        //}
    }
}
