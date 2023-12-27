using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services;
using EduQuiz_5P.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    [Area("Admin")]
    public class SubjectController : Controller
    {
        private ILogger<SubjectController> _logger;
        private ISubjectService _subjectService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        public SubjectController(ILogger<SubjectController> logger, IChapterService chapterService, IUserService userService, IClassService classService,
            ISubjectService subjectService)
        {
            _logger = logger;
            _subjectService = subjectService;
            _userService = userService;
            _classService = classService;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Subject> subjectLst = new List<Subject>();
            try
            {
                subjectLst = await _subjectService.GetListAsync();
                this.AddToastrMessage("Tải dữ liệu thành công.", Enums.ToastrMessageType.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage("Đã có lỗi xảy ra.", Enums.ToastrMessageType.Error);
            }
            return View(subjectLst);
        }

        public async Task<IActionResult> Create()
        {
            var classLst = await _classService.GetListAsync();
            ICollection<Subject> model = new List<Subject> { new Subject() { SelectClass = new SelectList(classLst, "Id", "ClassName") } };
            return View(model.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ICollection<Subject> subjects)
        {
            try
            {
                var user = await _userService.GetUser();
                if (user == null)
                {
                    return NotFound();
                }
                await _subjectService.AddRange(subjects, user.Id);
                this.AddToastrMessage("Tạo dữ liệu chương thành công.", Enums.ToastrMessageType.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage("Đã có lỗi xảy ra.", Enums.ToastrMessageType.Error);
            }
            return View(subjects);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            var classLst = await _classService.GetListAsync();
            subject.SelectClass = new SelectList(classLst, "Id", "ClassName");
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject model)
        {
            try
            {
                await _subjectService.Update(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Khi cập nhật Một Class : \n" + ex.ToString() + "\n\n");
            }
            return View(model);
        }
        public async Task<IActionResult> CreatePartial()
        {
            var classLst = await _classService.GetListAsync();
            ICollection<Subject> model = new List<Subject> { new Subject() { SelectClass = new SelectList(classLst, "Id", "ClassName") } };
            return PartialView("_DynamicAddSubject", model.ToList());
        }
    }
}
