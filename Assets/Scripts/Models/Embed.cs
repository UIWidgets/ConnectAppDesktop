using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class Embed
    {
        [JsonProperty("embedData")] public EmbedData embedData { get; set; }
        [JsonProperty("embedType")] public string embedType { get; set; }
    }
}