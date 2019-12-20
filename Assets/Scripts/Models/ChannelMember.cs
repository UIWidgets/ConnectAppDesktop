using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class ChannelMember
    {
        [JsonProperty("user")] public User user { get; set; }
        [JsonProperty("role")] public string role { get; set; }
    }
}