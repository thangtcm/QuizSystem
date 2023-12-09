namespace EduQuiz_5P.ViewModel
{
    public class ResponseListVM<T>
    {
        public List<T> Data { get; set; }
        public int MaxPage { get; set; }
    }
}
