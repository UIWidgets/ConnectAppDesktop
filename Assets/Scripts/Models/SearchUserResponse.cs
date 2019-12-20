using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class SearchUserResponse
    {
        [JsonProperty("items")] public List<User> items { get; set; }
        [JsonProperty("total")] public int total { get; set; }
    }
}