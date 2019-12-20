using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public abstract class Frame<T> : IFrame
    {
        [JsonProperty("op")] public int opCode { get; set; }
        [JsonProperty("s")] public int sequence { get; set; }
        [JsonProperty("t")] public string type { get; set; }
        [JsonProperty("d")] public T data { get; set; }
    }
}