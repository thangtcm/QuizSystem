using EduQuiz_5P.Enums;
using EduQuiz_5P.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.ViewModel
{
    public class ImportQuestionVM
    {
        public int? ImportChapterId { get; set; }
        public IFormFile? UploadFile { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public List<QuestionVM> QuestionVMs { get; set; }
    }
}
