using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class EmbedData
    {
        [JsonProperty("description")] public string description { get; set; }
        [JsonProperty("image")] public string image { get; set; }
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("title")] public string title { get; set; }
        [JsonProperty("url")] public string url { get; set; }
    }
}