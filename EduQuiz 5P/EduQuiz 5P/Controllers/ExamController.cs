using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EduQuiz_5P.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly ILogger<ExamController> _logger;
        private readonly IUserService _userService;
        private readonly ISubjectService _subjectService;
        public ExamController(IExamService examService, ILogger<ExamController> logger, IUserService userService, ISubjectService subjectService)
        {
            _examService = examService;
            _logger = logger;
            _userService = userService;
            _subjectService = subjectService;
        }
        public async Task<IActionResult> Index(int? page, int? ClassId = null, int subjectId = 1)
        {
            int pagesize = 16;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;
            ViewExamList model = new()
            {
                ExamList = new(new List<ExamInfoVM>(), pagenumber, pagesize),
                Subjects = new List<Subject>(),
                SelectSubject = subjectId
            };
            try
            {
                var user = await _userService.GetUser();
                var subjectlst = await _subjectService.GetListAsync();
                model.Subjects = subjectlst.ToList();
                if(user == null)
                {
          
                    var exams = (await _examService.GetExamDefaultList(ClassId, subjectId, null, x => x.Include(s => s.Subject!))).Select(x => new ExamInfoVM(x)).ToList();
                    model.ExamList = new PagedList<ExamInfoVM>(exams, pagenumber, pagesize);
                }
                else
                {
                    var exams = (await _examService.GetListAsync(SubjectId: subjectId, includes: x => x.Include(s => s.Subject!))).Select(x => new ExamInfoVM(x)).ToList();
                    model.ExamList = new PagedList<ExamInfoVM>(exams, pagenumber, pagesize);
                }
                this.AddToastrMessage("Tải dữ liệu thành công", Enums.ToastrMessageType.Success);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                this.AddToastrMessage("Đã có lỗi xảy ra", Enums.ToastrMessageType.Error);
            }
            return View(model);
        }
    }
}
