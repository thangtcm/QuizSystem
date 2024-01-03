using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.ViewModel
{
    public class DynamicSelect
    {
        public SelectList SelectListSubject { get; set; }
        public SelectList SelectListChapter { get; set; }
    }
}
