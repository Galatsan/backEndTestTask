using Newtonsoft.Json;

namespace backEndTestTask.Models
{
    public class Pictures
    {
        public string Id { get; set; }


        [JsonProperty("cropped_picture")]
        public string CroppedPicture { get; set; }
    }
}
