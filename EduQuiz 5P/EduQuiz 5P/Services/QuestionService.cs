using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using System.Text.RegularExpressions;

namespace EduQuiz_5P.Services
{
    public class QuestionService : IQuestionService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public QuestionService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        //public async Task Add(ImportQuestionVM model)
        //{
        //    ICollection<Question> questions = new List<Question>();
        //    Random random = new Random();
        //    var user = await _userService.GetUser();
        //    var listComponet = await _unitOfWork.ComponentChapterRespository.GetAllAsync(x => x.ChapterId == model.ChapterId);
        //    foreach(var questionvm in model.QuestionVMs)
        //    {
        //        int randomIndex = random.Next(0, listComponet.Count);
        //        Question question = new()
        //        {
        //            QuestionName = questionvm.QuestionName,
        //            QuestionHints = questionvm.QuestionHints,
        //            QuestionSolution = questionvm.QuestionSolution,
        //            IsImage = questionvm.IsImage,
        //            IsImageSolution = questionvm.IsImageSolution,
        //            ChappterId = model.ChapterId,
        //            ComponentChapterId = listComponet.ElementAt(randomIndex).Id,
        //            DateCreate = DateTime.Now,
        //            UserCreate = user
        //        };
        //        ICollection<Answer> answers = new List<Answer>();
        //        foreach(var ansVm in questionvm.AnswerVMs)
        //        {
        //            Answer answer = new()
        //            {
        //                Question = question,
        //                AnswerName = ansVm.AnswerName,
        //                IsCorrect = ansVm.IsCorrect,
        //                DateCreate = DateTime.Now,
        //                UserCreate = user
        //            };
        //            answers.Add(answer);
        //        }
        //        question.Answers = answers;
        //        questions.Add(question);
        //    }
        //    _unitOfWork.QuestionRepository.AddRange(questions);
        //    await _unitOfWork.CommitAsync();
        //}

        public async Task<bool> DeleteQuestion(int Id)
        {
            var question = GetById(Id);
            if (question == null) return false;
            try
            {
                _unitOfWork.QuestionRepository.Remove(question);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public Question? GetById(int id)
            => _unitOfWork.QuestionRepository.Get(x => x.Id == id);

        public async Task<Question?> GetQuestionByIdAsync(int id)
            => await _unitOfWork.QuestionRepository.GetAsync(x => x.Id == id);

        public async Task<ICollection<Question>> GetQuestions()
            => await _unitOfWork.QuestionRepository.GetAllAsync();

        public async Task<bool> Update(Question question)
        {
            try
            {
                _unitOfWork.QuestionRepository.Update(question);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        //public async Task<ICollection<QuestionVM>> ReadFileLatex(IFormFile file)
        //{
        //    ICollection<QuestionVM> questions = new List<QuestionVM>();
        //    if (file != null && file.Length > 0)
        //    {
        //        if (Path.GetExtension(file.FileName) == ".tex")
        //        {
        //            using (var reader = new StreamReader(file.OpenReadStream()))
        //            {
        //                var fileContent = await reader.ReadToEndAsync();
        //                fileContent = fileContent.Replace(@"%[]", "");
        //                string exPattern = @"\\begin{ex}([\s\S]*?)\\end{ex}";
        //                MatchCollection exMatches = Regex.Matches(fileContent, exPattern, RegexOptions.Multiline);
        //                foreach (Match match in exMatches.Cast<Match>())
        //                {
        //                    QuestionVM question = new QuestionVM();
                            
        //                    string Sesstion = match.Groups[1].Value.Trim() + @"\end{loigiai}"; ;
        //                    int indexAnswer = Sesstion.IndexOf("\\choice");
        //                    string QuestionName = Sesstion.Substring(0, indexAnswer);
        //                    string tikzPattern = @"\\begin{tikzpicture}(.*?)\\end{tikzpicture}";
        //                    Match tikzMatch = Regex.Match(QuestionName, tikzPattern, RegexOptions.Singleline);

        //                    if (tikzMatch.Success)
        //                    {
        //                        string tikzContent = @"\begin{tikzpicture}" + tikzMatch.Groups[1].Value + @"\end{tikzpicture}";
        //                        QuestionName = QuestionName.Replace(tikzContent, "").Replace(@"\centerline{", "").Replace(@"}\\", "").Replace(@"\begin{center}", "").Replace(@"\end{center}", "");
        //                        question.IsImage = "https://i.upmath.me/svg/" + Uri.EscapeDataString(tikzContent.Trim());
        //                    }
        //                    question.QuestionName = QuestionName.Replace(@"\immini[thm]{", "").Trim();
        //                    string answerPattern = @"\\choice([\s\S]*?)\\loigiai\{";
        //                    MatchCollection answerMatches = Regex.Matches(Sesstion, answerPattern);

        //                    foreach (Match answerMatch in answerMatches)
        //                    {
        //                        ICollection<AnswerVM> answers = new List<AnswerVM>();
        //                        string Fullanswer = answerMatch.Groups[1].Value.Trim();
        //                        string[] lines = Fullanswer.Split('\n');

        //                        foreach (string line in lines)
        //                        {
        //                            AnswerVM answer = new()
        //                            {
        //                                AnswerName = line.Trim().TrimStart('{').TrimEnd('}')
        //                            };
        //                            if(answer.AnswerName.Contains(@"\True"))
        //                            {
        //                                answer.IsCorrect= true;
        //                                answer.AnswerName = answer.AnswerName.Replace(@"\True", "").Trim();
        //                            }    
        //                            answers.Add(answer);
        //                        }
        //                        question.AnswerVMs = answers;
        //                    }
        //                    string solutionPattern = @"\\loigiai{([\s\S]*?)}\\end{loigiai}";

        //                    MatchCollection solutionMatches = Regex.Matches(Sesstion, solutionPattern);
        //                    foreach (Match solution in solutionMatches)
        //                    {
        //                        string solutionContent = solution.Groups[1].Value.Trim();
        //                        tikzMatch = Regex.Match(solutionContent, tikzPattern, RegexOptions.Singleline);
        //                        if (tikzMatch.Success)
        //                        {
        //                            string IsImageSolution = "\\begin{tikzpicture}" + tikzMatch.Groups[1].Value + "\\end{tikzpicture}";
        //                            question.IsImageSolution = "https://i.upmath.me/svg/" + Uri.EscapeDataString(IsImageSolution.Trim());
        //                            solutionContent = solutionContent.Replace(IsImageSolution, "").Replace(@"\centerline{", "").Replace(@"}\\", "").Replace(@"\begin{center}", "").Replace(@"\end{center}", "");
        //                        }
        //                        question.QuestionSolution = solutionContent;
        //                    }
        //                    questions.Add(question);
        //                }
        //            }
        //        }
        //    }
        //    return questions;
        //}
    }
}
