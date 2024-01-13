using EduQuiz_5P.Models;
using X.PagedList;

namespace EduQuiz_5P.ViewModel
{
    public class ViewExamList
    {
        public List<Subject> Subjects { get; set; }
        public PagedList<ExamInfoVM> ExamList { get; set; }
        public int SelectSubject { get; set; }
    }
}
