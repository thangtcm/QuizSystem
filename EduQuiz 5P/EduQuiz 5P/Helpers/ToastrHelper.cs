using Microsoft.AspNetCore.Mvc;
using EduQuiz_5P.Enums;

namespace EduQuiz_5P.Helpers
{
    public static class ToastrHelper
    {
        public static void AddToastrMessage(this Controller controller, string message, ToastrMessageType type)
        {
            controller.TempData["ToastrMessage"] = message;
            controller.TempData["ToastrMessageType"] = type.ToString().ToLower();
        }
    }

}
