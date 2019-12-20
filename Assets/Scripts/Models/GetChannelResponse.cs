using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class GetChannelResponse
    {
        [JsonProperty("channel")] public Channel channel { get; set; }
        [JsonProperty("groupFull")] public Group groupFull { get; set; }
    }
}