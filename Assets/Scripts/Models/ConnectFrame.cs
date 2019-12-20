using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class ConnectFrame
    {
        [JsonProperty("op")] public int opCode { get; set; }
        [JsonProperty("d")] public ConnectFrameData data { get; set; }
    }

    public class ConnectFrameData
    {
        [JsonProperty("commitId")] public string commitId { get; set; }
        [JsonProperty("properties")] public Dictionary<string, object> properties { get; set; }
        [JsonProperty("clientType")] public string clientType { get; set; }
        [JsonProperty("isApp")] public bool isApp { get; set; }
    }
}