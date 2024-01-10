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
        public ExamType ExamType { get; set; }
        public List<QuestionVM> QuestionVMs { get; set; }
    }
}
