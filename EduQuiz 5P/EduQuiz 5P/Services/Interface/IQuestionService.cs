﻿using EduQuiz_5P.Models;
using EduQuiz_5P.ViewModel;

namespace EduQuiz_5P.Services.Interface
{
    public interface IQuestionService
    {
        public Task<ICollection<Question>> GetQuestions();
        public Task<int> CountAsync();
        public Task Add(ImportQuestionVM model);
        public Task AddAPI(ICollection<Question> model, int chapterId);
        public Task<Question?> GetQuestionByIdAsync(int id);
        public Task<ResponResultData<Question>> GenerateQuestion(int examMatrixId);
        public Question? GetById(int id);
        public Task<bool> DeleteQuestion(int Id);
        public Task<bool> Update(Question question);
        public Task<ICollection<QuestionVM>> ReadFileLatex(IFormFile file);
        public Task<ICollection<QuestionVM>> ReadFileDoc(IFormFile file);
    }
}
