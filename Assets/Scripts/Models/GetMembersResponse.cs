using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class GetMembersResponse
    {
        [JsonProperty("list")] public List<ChannelMember> list { get; set; }
        [JsonProperty("total")] public int total { get; set; }
    }
}