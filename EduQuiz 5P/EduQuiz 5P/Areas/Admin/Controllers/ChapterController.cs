using DocumentFormat.OpenXml.VariantTypes;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services;
using EduQuiz_5P.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    [Area("Admin")]
    public class ChapterController : Controller
    {
        private ILogger<ChapterController> _logger;
        private IChapterService _chapterService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        public ChapterController(ILogger<ChapterController> logger, IChapterService chapterService, IUserService userService, 
            IClassService classService, ISubjectService subjectService)
        {
            _logger = logger;
            _chapterService = chapterService;
            _userService = userService;
            _classService = classService;
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index(int? classId = null,int? subjectId = null)
        {
            ICollection<Chapter> Chapterlst = new List<Chapter>();
            try
            {
                Chapterlst = await _chapterService.GetListAsync(classId, subjectId, x => x.Include(s => s.Subject));
                this.AddToastrMessage("Tải dữ liệu thành công.", Enums.ToastrMessageType.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage("Đã có lỗi xảy ra.", Enums.ToastrMessageType.Error);
            }
            var classLst = await _classService.GetListAsync();
            var subjectLst = await _subjectService.GetListAsync();
            ViewData["ClassList"] = new SelectList(classLst, "Id", "ClassName", classId);
            ViewData["SubjectList"] = new SelectList(subjectLst, "Id", "SubjectName", subjectId);
            return View(Chapterlst);
        }

        public async Task<IActionResult> Create()
        {
            var classLst = await _classService.GetListAsync();
            var subjectLst = await _subjectService.GetListAsync();
            ICollection<Chapter> chapters = new List<Chapter>()
            {
                new Chapter() {SelectClass = new SelectList(classLst, "Id", "ClassName"), SelectSubject = new SelectList(subjectLst, "Id", "SubjectName")}
            };
            return View(chapters.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ICollection<Chapter> chapters)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    return NotFound();
                }
                await _chapterService.AddRange(chapters, user.Id);
                this.AddToastrMessage("Tạo dữ liệu chương thành công.", Enums.ToastrMessageType.Success);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage("Đã có lỗi xảy ra.", Enums.ToastrMessageType.Error);
            }
            return View(chapters);
        }

        public IActionResult Edit(int? id)
        {
            var subject = _subjectService.GetById(id);
            if (subject == null)
            {
                return NotFound();
            }
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
            var subjectLst = await _subjectService.GetListAsync();
            ICollection<Chapter> chapters = new List<Chapter>()
            {
                new Chapter() {SelectClass = new SelectList(classLst, "Id", "ClassName"), SelectSubject = new SelectList(subjectLst, "Id", "SubjectName")}
            };
            return PartialView("_DynamicAddChapter", chapters.ToList());
        }

        public async Task<IActionResult> LoadChapter(int? classId, int? subjectId)
        {
            var chapterlst = await _chapterService.GetListAsync(classId, subjectId, x => x.Include(c => c.Classes!).Include(s => s.Subject!));
            var selectList = new SelectList(chapterlst, "Id", "DisplayText");
            return Json(selectList);
        }
    }
}
