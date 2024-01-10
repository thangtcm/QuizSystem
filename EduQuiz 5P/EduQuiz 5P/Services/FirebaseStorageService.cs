using EduQuiz_5P.Services.Interface;
using Firebase.Auth;
using Firebase.Storage;

namespace EduQuiz_5P.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfiguration _configuration;
        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Uri> UploadFile(IFormFile upload)
        {
            var storage = string.Empty;
            if (upload != null && upload.Length > 0)
            {
                List<string> list = new()
                {
                    "image/bmp",
                    "image/gif",
                    "image/jpeg",
                    "image/png",
                    "image/svg+xml",
                    "image/tiff",
                    "image/webp"
                };
                if (list.Contains(upload.ContentType))
                {
                    var randomGuid = Guid.NewGuid();
                    using var stream = new MemoryStream();
                    await upload.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    var bucketName = _configuration["Firebase:BucketName"];
                    var apiKey = _configuration["Firebase:APIKey"];
                    var userName = _configuration["Firebase:AccEmail"];
                    var password = _configuration["Firebase:AccPassword"];
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var account = await auth.SignInWithEmailAndPasswordAsync(userName, password);
                    var cancelToken = new CancellationTokenSource();
                    var objectName = $"{Path.GetFileNameWithoutExtension(upload.FileName)}-{randomGuid}{Path.GetExtension(upload.FileName)}";
                    // Tạo một client Firebase Storage
                    storage = await new FirebaseStorage(bucketName, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = async () => await Task.FromResult(account.FirebaseToken),
                        ThrowOnCancel = true
                    }).Child("Images").Child(objectName).PutAsync(stream, cancelToken.Token);
                }
            }
            return new Uri(storage);
        }

    }
}
