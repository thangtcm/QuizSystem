using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
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
        private readonly IFirebaseStorageService _firebaseStorageService;
        public QuestionService(IUnitOfWork unitOfWork, IUserService userService, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _firebaseStorageService = firebaseStorageService;
        }
        public async Task Add(ImportQuestionVM model)
        {
            ICollection<Question> questions = new List<Question>();
            ICollection<Answer> answers = new List<Answer>();
            var user = await _userService.GetUser();
            foreach (var questionvm in model.QuestionVMs)
            {
                var urlImageQuestion = questionvm.UrlImage;
                var urlImageSolution = questionvm.UrlImageSolution;
                if (questionvm.UploadImageQuestion != null)
                {
                    urlImageQuestion = (await _firebaseStorageService.UploadFile(questionvm.UploadImageQuestion)).ToString();
                }    
                if(questionvm.UploadImageQuestionSolution != null)
                {
                    urlImageSolution = (await _firebaseStorageService.UploadFile(questionvm.UploadImageQuestionSolution)).ToString();
                }
                Question question = new()
                {
                    QuestionName = questionvm.QuestionName,
                    QuestionHints = questionvm.QuestionHints,
                    QuestionSolution = questionvm.QuestionSolution,
                    IsImage = urlImageQuestion,
                    IsImageSolution = urlImageSolution,
                    ChappterId = model.ImportChapterId,
                    DateUpdate = DateTime.UtcNow.ToTimeZone(),
                    DifficultyLevel = model.DifficultyLevel,
                    UserUpdate = user
                };
                questions.Add(question);
                foreach (var ansVm in questionvm.AnswerList)
                {
                    Answer answer = new()
                    {
                        Question = question,
                        AnswerName = ansVm.AnswerName,
                        IsCorrect = ansVm.IsCorrect,
                        DateUpdate = DateTime.UtcNow.ToTimeZone(),
                        UserUpdate = user
                    };
                    answers.Add(answer);
                }
            }
            _unitOfWork.QuestionRepository.AddRange(questions);
            _unitOfWork.AnswerRepository.AddRange(answers);
            await _unitOfWork.CommitAsync();
        }


        public async Task AddAPI(ICollection<Question> model, int chapterId)
        {
            Random random = new();

            foreach(var item in model)
            {
                item.ChappterId = chapterId;
                item.DateUpdate = DateTime.UtcNow.ToTimeZone();
                item.DifficultyLevel = (DifficultyLevel)(random.Next(4) + 1);
                item.UserIdUpdate = 1;
                foreach(var answer in item.Answers!)
                {
                    answer.DateUpdate = DateTime.UtcNow.ToTimeZone();
                    answer.UserIdUpdate = 1;
                }    
            }
            await _unitOfWork.QuestionRepository.AddRangeAsync(model);
            await _unitOfWork.CommitAsync();
        }
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

        public async Task<ICollection<QuestionVM>> ReadFileDoc(IFormFile file)
        {
            List<QuestionVM> questions = new ();
            QuestionVM currentQuestion = new()
            {
                QuestionName = "",
                AnswerList = new List<AnswerVMInfo>()
            };
            if (file != null && file.Length > 0)
            {
                if (Path.GetExtension(file.FileName) == ".docx")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, false))
                        {
                            var body = doc.MainDocumentPart?.RootElement?.Descendants<Paragraph>();
                            if(body != null)
                            {
                                foreach (var paragraph in body)
                                {
                                    string text = paragraph.InnerText.Trim();
                                    if (text.StartsWith("Câu") && text.Contains(':'))
                                    {
                                        Match match = Regex.Match(text, @"^Câu (\d+): (.+)$");
                                        if (match.Success)
                                        {
                                            currentQuestion = new QuestionVM
                                            {
                                                QuestionName = match.Groups[2].Value.Trim(),
                                                AnswerList = new List<AnswerVMInfo>()
                                            };
                                        }
                                        else
                                        {
                                            currentQuestion = new QuestionVM
                                            {
                                                QuestionName = text,
                                                AnswerList = new List<AnswerVMInfo>()
                                            };
                                        }
                                        questions.Add(currentQuestion);
                                    }
                                    else if (currentQuestion != null && text.StartsWith("A.") || text.StartsWith("B.") || text.StartsWith("C.") || text.StartsWith("D."))
                                    {
                                        // Add answers to the current question
                                        var answer = new AnswerVMInfo
                                        {
                                            AnswerName = text.Substring(3).Trim(),
                                        };
                                        currentQuestion!.AnswerList.Add(answer);
                                    }
                                }
                            }
                            var tables = doc.MainDocumentPart?.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ToList();
                            if(tables != null)
                            {
                                var table = tables.Last();
                                if (table != null)
                                {
                                    int questionIndex, answerIndex;
                                    for (int row = 0; row < table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Count(); row++)
                                    {
                                        var tableRow = table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(row);
                                        for (int col = 0; col < tableRow.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().Count(); col++)
                                        {
                                            var cell = tableRow.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(col);
                                            string cellText = cell.InnerText.Trim();

                                            var result = ParseQuestionString(cellText);
                                            questionIndex = result.questionIndex;
                                            answerIndex = result.answerIndex;
                                            questions[questionIndex - 1].AnswerList.ElementAt(answerIndex).IsCorrect = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return questions;
        }

        public (int questionIndex, int answerIndex) ParseQuestionString(string questionString)
        {
            string[] parts = questionString.Split('.');

            if (parts.Length == 2)
            {
                if (int.TryParse(parts[0], out int questionIndex))
                {
                    char answerChar = parts[1].Trim().ToUpper()[0];
                    int answerIndex = answerChar - 'A';

                    return (questionIndex, answerIndex);
                }
            }

            // Trong trường hợp không thành công, trả về giá trị mặc định
            return (-1, -1);
        }

        public async Task<ICollection<QuestionVM>> ReadFileLatex(IFormFile file)
        {
            ICollection<QuestionVM> questions = new List<QuestionVM>();
            if (file != null && file.Length > 0)
            {
                if (Path.GetExtension(file.FileName) == ".tex")
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var fileContent = await reader.ReadToEndAsync();
                        fileContent = fileContent.Replace(@"%[]", "");
                        string exPattern = @"\\begin{ex}([\s\S]*?)\\end{ex}";
                        MatchCollection exMatches = Regex.Matches(fileContent, exPattern, RegexOptions.Multiline);
                        foreach (Match match in exMatches.Cast<Match>())
                        {
                            QuestionVM question = new QuestionVM();

                            string Sesstion = match.Groups[1].Value.Trim() + @"\end{loigiai}"; ;
                            int indexAnswer = Sesstion.IndexOf("\\choice");
                            string QuestionName = Sesstion.Substring(0, indexAnswer);
                            string tikzPattern = @"\\begin{tikzpicture}(.*?)\\end{tikzpicture}";
                            Match tikzMatch = Regex.Match(QuestionName, tikzPattern, RegexOptions.Singleline);

                            if (tikzMatch.Success)
                            {
                                string tikzContent = @"\begin{tikzpicture}" + tikzMatch.Groups[1].Value + @"\end{tikzpicture}";
                                QuestionName = QuestionName.Replace(tikzContent, "").Replace(@"\centerline{", "").Replace(@"}\\", "").Replace(@"\begin{center}", "").Replace(@"\end{center}", "");
                                question.UrlImage = "https://i.upmath.me/svg/" + Uri.EscapeDataString(tikzContent.Trim());
                            }
                            question.QuestionName = QuestionName.Replace(@"\immini[thm]{", "").Trim();
                            string answerPattern = @"\\choice([\s\S]*?)\\loigiai\{";
                            MatchCollection answerMatches = Regex.Matches(Sesstion, answerPattern);

                            foreach (Match answerMatch in answerMatches)
                            {
                                ICollection<AnswerVMInfo> answers = new List<AnswerVMInfo>();
                                string Fullanswer = answerMatch.Groups[1].Value.Trim();
                                string[] lines = Fullanswer.Split('\n');

                                foreach (string line in lines)
                                {
                                    AnswerVMInfo answer = new()
                                    {
                                        AnswerName = line.Trim().TrimStart('{').TrimEnd('}')
                                    };
                                    if (answer.AnswerName.Contains(@"\True"))
                                    {
                                        answer.IsCorrect = true;
                                        answer.AnswerName = answer.AnswerName.Replace(@"\True", "").Trim();
                                    }
                                    answers.Add(answer);
                                }
                                question.AnswerList = answers.ToList();
                            }
                            string solutionPattern = @"\\loigiai{([\s\S]*?)}\\end{loigiai}";

                            MatchCollection solutionMatches = Regex.Matches(Sesstion, solutionPattern);
                            foreach (Match solution in solutionMatches)
                            {
                                string solutionContent = solution.Groups[1].Value.Trim();
                                tikzMatch = Regex.Match(solutionContent, tikzPattern, RegexOptions.Singleline);
                                if (tikzMatch.Success)
                                {
                                    string IsImageSolution = "\\begin{tikzpicture}" + tikzMatch.Groups[1].Value + "\\end{tikzpicture}";
                                    question.UrlImageSolution = "https://i.upmath.me/svg/" + Uri.EscapeDataString(IsImageSolution.Trim());
                                    solutionContent = solutionContent.Replace(IsImageSolution, "").Replace(@"\centerline{", "").Replace(@"}\\", "").Replace(@"\begin{center}", "").Replace(@"\end{center}", "");
                                }
                                question.QuestionSolution = solutionContent;
                            }
                            questions.Add(question);
                        }
                    }
                }
            }
            return questions;
        }
    }
}
