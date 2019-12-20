using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class ReadyFrame : Frame<ReadyFrameData>
    {

    }
    
    public class ReadyFrameData
    {
        [JsonProperty("lobbyChannels")] public List<Channel> lobbyChannels { get; set; }
        [JsonProperty("lastMessages")] public List<Message> lastMessages { get; set; }
        [JsonProperty("users")] public List<User> users { get; set; }
        [JsonProperty("userId")] public string userId { get; set; }
        [JsonProperty("readState")] public List<ReadState> readStates { get; set; }
        [JsonProperty("groupMap")] public Dictionary<string, Group> groupMap { get; set; }
    }
}