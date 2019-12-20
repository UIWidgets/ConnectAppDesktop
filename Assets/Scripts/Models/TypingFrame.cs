using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class TypingFrame : Frame<TypingFrameData>
    {
        
    }

    public class TypingFrameData
    {
        [JsonProperty("channelId")] public string channelId { get; set; }
        [JsonProperty("timestamp")] public long timestamp { get; set; }
        [JsonProperty("userId")] public string userId { get; set; }
    }
}