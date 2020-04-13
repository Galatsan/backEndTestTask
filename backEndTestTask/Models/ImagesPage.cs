namespace backEndTestTask.Models
{
    public class ImagesPage
    {
        public int Page { get; set; }

        public int PageCount { get; set; }

        public bool HasMore { get; set; }

        public Pictures[] Pictures { get; set; }
    }
}
