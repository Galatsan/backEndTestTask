namespace backEndTestTask.Models.Responses
{
    public class ImagesPageResponse
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public bool HasMore { get; set; }
        public PicturesResponse[] Pictures { get; set; }
    }
}
