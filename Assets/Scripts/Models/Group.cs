using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class Group
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("description")] public string description { get; set; }
        [JsonProperty("privacy")] public string privacy { get; set; }
    }
}