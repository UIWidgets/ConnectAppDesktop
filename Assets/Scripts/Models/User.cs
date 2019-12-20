using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class User
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("fullname")] public string fullName { get; set; }
        [JsonProperty("avatar")] public string avatar { get; set; }
        [JsonProperty("title")] public string title { get; set; }
    }
}