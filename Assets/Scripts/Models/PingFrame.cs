using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class PingFrame : Frame<PingFrameData>
    {
    }

    public class PingFrameData
    {
        [JsonProperty("ts")] public long timeStamp { get; set; }
        [JsonProperty("cid")] public string currentChannelId { get; set; }
    }
}