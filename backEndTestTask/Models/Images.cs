using backEndTestTask.Attributes;
using Newtonsoft.Json;

namespace backEndTestTask.Models
{
    public class Images
    {
        public string Id { get; set; }

        [SearchField]
        public string Author { get; set; }

        [SearchField]
        public string Camera { get; set; }

        [SearchField]
        public string Tags { get; set; }

        [JsonProperty("cropped_picture")]
        public string CroppedPicture { get; set; }

        [JsonProperty("full_picture")]
        public string FullPicture { get; set; }  
    }
}
