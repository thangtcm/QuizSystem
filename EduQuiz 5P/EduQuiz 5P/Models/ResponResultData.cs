namespace EduQuiz_5P.Models
{
    public class ResponResultData<T>
    {
        public T? Result { get; set; }
        public ICollection<T>? ListResult { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
