using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Unity.Messenger.Models
{
    public class ReadState
    {
        [JsonProperty("channelId")] public string channelId { get; set; }
        [JsonProperty("lastMentionId")] public string lastMentionId { get; set; }
        [JsonProperty("lastMessageId")] public string lastMessageId { get; set; }
        [JsonProperty("mentionCount")] public int mentionCount { get; set; }
    }
}