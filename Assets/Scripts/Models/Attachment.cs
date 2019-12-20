using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class Attachment
    {
        [JsonProperty("contentType")] public string contentType { get; set; }
        [JsonProperty("url")] public string url { get; set; }
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("height")] public int height { get; set; }
        [JsonProperty("width")] public int width { get; set; }
        [JsonProperty("signedUrl")] public string signedUrl { get; set; }
        [JsonProperty("filename")] public string filename { get; set; }
        [JsonProperty("size")] public int size { get; set; }
        public bool local { get; set; }
        public System.Func<float> progress { get; set; } 
    }
}