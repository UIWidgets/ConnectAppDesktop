using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class Channel
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("type")] public string type { get; set; }
        [JsonProperty("workspaceId")] public string workspaceId { get; set; }
        [JsonProperty("thumbnail")] public string thumbnail { get; set; }
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("memberCount")] public int memberCount { get; set; }
        [JsonProperty("topic")] public string topic { get; set; }
        [JsonProperty("lastMessage")] public Message lastMessage { get; set; }
        [JsonProperty("lastMessageId")] public string lastMessageId { get; set; }
        [JsonProperty("liveChannelId")] public string liveChannelId { get; set; }
        [JsonProperty("isMute")] public bool isMute { get; set; }
        [JsonProperty("groupId")] public string groupId { get; set; }
        [JsonProperty("stickTime")] public string stickTime { get; set; }
        [JsonProperty("groupMap")] public Dictionary<string, Group> groupMap { get; set; }
    }
}