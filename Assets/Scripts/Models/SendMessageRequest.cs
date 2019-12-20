using System.Collections.Generic;
using Newtonsoft.Json;
using static Unity.Messenger.Utils;

namespace Unity.Messenger.Models
{
    public class SendMessageRequest
    {
        public SendMessageRequest(
            string content,
            string nonce,
            List<User> mentions,
            string parentMessageId = "",
            string quoteMessageId = null)
        {
            this.content = content;
            this.parentMessageId = parentMessageId;
            this.quoteMessageId = quoteMessageId;
            this.nonce = nonce;
            this.mentions = mentions;
        }
        
        [JsonProperty("mentions")] public List<User> mentions { get; private set; }
        [JsonProperty("content")] public string content { get; private set; }
        [JsonProperty("parentMessageId")] public string parentMessageId { get; private set; }
        [JsonProperty("quoteMessageId")] public string quoteMessageId { get; private set; }
        [JsonProperty("nonce")] public string nonce { get; private set; }
    }
}