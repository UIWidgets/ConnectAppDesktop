using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class GetMessagesResponse
    {
        [JsonProperty("hasMore")] public bool hasMore { get; set; }
        [JsonProperty("hasMoreNew")] public bool hasMoreNew { get; set; }
        [JsonProperty("items")] public List<Message> items { get; set; }
        [JsonProperty("parents")] public List<Message> parents { get; set; }
    }
}