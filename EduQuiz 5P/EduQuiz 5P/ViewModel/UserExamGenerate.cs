using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.ViewModel
{
    public class UserExamGenerate
    {
        public int ExamTime { get; set; }
        public int NumberOfQuestion { get; set; }
        public int? ExamMatrixId { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public SelectList SelectClass { get; set; }
        public SelectList SelectSubject { get; set; }
        public UserExamGenerate() { }
        public UserExamGenerate(SelectList SelectClass, SelectList SelectSubject) { 
            this.SelectClass = SelectClass;
            this.SelectSubject = SelectSubject;
        }
    }
}
