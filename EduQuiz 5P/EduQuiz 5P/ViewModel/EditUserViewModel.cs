using Microsoft.AspNetCore.Mvc.Rendering;
using EduQuiz_5P.Data;

namespace EduQuiz_5P.ViewModel
{
    public class EditUserViewModel
    {
        public ApplicationUser? User { get; set; }
        public IList<SelectListItem>? Roles { get; set; }
    }
}
