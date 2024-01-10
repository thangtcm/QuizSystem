namespace EduQuiz_5P.Services.Interface
{
    public interface IFirebaseStorageService
    {
        public Task<Uri> UploadFile(IFormFile file);
    }
}
