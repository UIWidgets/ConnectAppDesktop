using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity.Messenger.Models
{
    public class Message
    {
        public bool sendError { get; set; }
        
        [JsonProperty("channelId")] public string channelId { get; set; }
        [JsonProperty("content")] public string content { get; set; }
        [JsonProperty("type")] public string type { get; set; }
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("nonce")] public string nonce { get; set; }
        [JsonProperty("author")] public User author { get; set; }
        [JsonProperty("attachments")] public List<Attachment> attachments { get; set; }
        [JsonProperty("quoteMessageId")] public string quoteMessageId { get; set; }
        [JsonProperty("parentMessageId")] public string parentMessageId { get; set; }
        [JsonProperty("embeds")] public List<Embed> embeds { get; set; }
        [JsonProperty("mentions")] public List<User> mentions { get; set; }
        [JsonProperty("mentionEveryone")] public bool mentionEveryone { get; set; }
        [JsonProperty("deletedTime")] public string deletedTime { get; set; }
        [JsonProperty("lastEditedId")] public string lastEditedId { get; set; }
    }
}