using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class JoinChannelResponse
    {
        [JsonProperty("member")] public ChannelMember member { get; set; }
        [JsonProperty("channel")] public Channel channel { get; set; }
    }
}