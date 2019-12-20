using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class EditorSessionTokenResponse
    {
        [JsonProperty("LSToken")] public string loginSessionToken { get; set; }
        [JsonProperty("userId")] public string userId { get; set; }
    }
}