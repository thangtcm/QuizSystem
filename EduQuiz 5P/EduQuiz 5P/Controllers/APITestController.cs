using EduQuiz_5P.Models;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduQuiz_5P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APITestController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public APITestController(IExamService examService, IFirebaseStorageService firebaseStorageService)
        {
            _examService = examService;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpPost("Testupload")]
        public async Task<IActionResult> Testupload(IFormFile upload)
        {
            try
            {
                var result = await _firebaseStorageService.UploadFile(upload);
                return Ok(result.ToString());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return NotFound();
        }

        //[HttpPost("RandomQuestion")]
        //public async Task<IActionResult> RandomQuestion(ExamInfoVM exam)
        //{
        //    try
        //    {
        //        Exam model = new()
        //        {
        //            ExamName = exam.ExamName,
        //            ExamDescription = exam.ExamDescription,
        //            ExamTime = exam.ExamTime,
        //        };
        //        var result = await _examService.CreateExamWithMatrix(model, 2, 1);
        //        if (result.IsSuccess)
        //        {
        //            return Ok(result.Result!.Questions);
        //        }
        //        else
        //        {
        //            return BadRequest(result.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return NotFound();
        //}
    }
}
