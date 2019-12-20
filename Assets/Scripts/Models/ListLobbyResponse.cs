using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class ListLobbyResponse
    {
        [JsonProperty("currentPage")] public int currentPage { get; set; }
        [JsonProperty("items")] public List<Channel> items { get; set; }
        [JsonProperty("pages")] public List<int> pages { get; set; }
        [JsonProperty("total")] public int total { get; set; }
    }
}