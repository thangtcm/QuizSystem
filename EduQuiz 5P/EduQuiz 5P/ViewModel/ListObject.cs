namespace EduQuiz_5P.ViewModel
{
    public class ListObject<T>
    {
        public ICollection<T> Data { get; set; } = new List<T>();
    }
}
