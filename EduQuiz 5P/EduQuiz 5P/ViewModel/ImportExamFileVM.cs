using EduQuiz_5P.Enums;

namespace EduQuiz_5P.ViewModel
{
    public class ImportExamFileVM
    {
        public string ExamName { get; set; }
        public string ExamDescription { get; set; }
        public int ExamTime { get; set; }
        public int ExamClassId { get; set; }
        public int ExamSubjectId { get; set; }
        public double ExamIdentification { get; set; }
        public double ExamUnderstanding { get; set; }
        public double ExamApplication { get; set; }
        public double ExamAdvancedApplication { get; set; }
        public ExamType ExamType { get; set; }
        public IFormFile? FileUpload { get; set; }
        public List<QuestionVM> QuestionVMs { get; set; }
    }
}
