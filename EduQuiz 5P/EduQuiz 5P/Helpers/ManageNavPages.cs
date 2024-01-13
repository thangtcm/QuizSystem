using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.Helpers
{
    public static class ManageNavPages
    {
        public static string Home => "Home";
        public static string About => "Home";
        public static string ExamNav => "Exam";


        public static string MainNavClass(ViewContext viewContext, string controllerName, string? actionName = null, string? areaName = null)
        {
            var currentController = viewContext.RouteData.Values["controller"] as string;
            var currentAction = viewContext.RouteData.Values["action"] as string;
            var currentArea = viewContext.RouteData.Values["area"] as string;

            if (string.Equals(currentController, controllerName, StringComparison.OrdinalIgnoreCase) &&
                (actionName == null || string.Equals(currentAction, actionName, StringComparison.OrdinalIgnoreCase)) &&
                (areaName == null || string.Equals(currentArea, areaName, StringComparison.OrdinalIgnoreCase)))
            {
                return "active";
            }

            return "";
        }

        public static string MainNavMutipleClass(ViewContext viewContext, List<string> controllerNames, string? actionName = null, string? areaName = null)
        {
            var currentController = viewContext.RouteData.Values["controller"] as string;
            var currentAction = viewContext.RouteData.Values["action"] as string;
            var currentArea = viewContext.RouteData.Values["area"] as string;

            if (controllerNames.Contains(currentController ?? "") &&
            (actionName == null || string.Equals(currentAction, actionName, StringComparison.OrdinalIgnoreCase)) &&
            (areaName == null || string.Equals(currentArea, areaName, StringComparison.OrdinalIgnoreCase)))
            {
                return "active";
            }

            return "";
        }
    }
}
