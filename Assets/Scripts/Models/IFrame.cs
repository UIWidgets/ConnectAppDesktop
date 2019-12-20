using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    [JsonConverter(typeof(FrameConverter))]
    public interface IFrame
    {
    }
}