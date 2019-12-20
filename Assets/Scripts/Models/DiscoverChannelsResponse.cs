using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Unity.Messenger.Models
{
    public class DiscoverChannelsResponse
    {
        [JsonProperty("discoverList")] public List<string> discoverList { get; set; }
        [JsonProperty("joinedList")] public List<string> joinedList { get; set; }
        [JsonProperty("channelMap")] public Dictionary<string, Channel> channelMap { get; set; }
        [JsonProperty("groupFullMap")] public Dictionary<string, Group> groupFullMap { get; set; } 
    }
}