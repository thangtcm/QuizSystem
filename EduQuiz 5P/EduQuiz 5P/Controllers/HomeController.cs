using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace EduQuiz_5P.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        public HomeController(ILogger<HomeController> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Test(string UrlAPI, int chapterId)
        //{
        //    try
        //    {
        //        var response = await _client.GetAsync(UrlAPI);
        //        List<Question> model = new List<Question>();
        //        Random random = new();
        //        Console.WriteLine("Chapter " + chapterId);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //            var jsonObject = JObject.Parse(jsonString);
        //            foreach (var item in jsonObject["data"])
        //            {
        //                Question quesiton = new()
        //                {
        //                    QuestionName = item["QuestionName"].ToString(),
        //                    IsImage = item["IsImage"].ToString(),
        //                    QuestionSolution = item["QuestionSolution"].ToString(),
        //                    IsImageSolution = item["IsImageSolution"].ToString(),
        //                };

        //                ICollection<Answer> answers = new List<Answer>();
        //                // Process answers if needed
        //                foreach (var answer in item["answers"])
        //                {
        //                    Answer answerAnswer = new()
        //                    {
        //                        AnswerName = answer["id"].ToString(),
        //                        IsCorrect = (bool)answer["isCorrect"],
        //                    };
        //                    answers.Add(answerAnswer);
        //                }
        //                quesiton.Answers = answers;
        //                model.Add(quesiton);
        //            }
        //            await _questionService.AddAPI(model, chapterId);
        //        }
        //    }catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
            
        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}