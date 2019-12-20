using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Unity.Messenger.Models
{
    public class FrameConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(Frame<>));
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["t"].ToString();
            var dataReader = jObject["d"].CreateReader();
            var opCode = int.Parse(jObject["op"].ToString());
            var sequence = int.Parse(jObject["s"].ToString());
            switch (type)
            {
                case "READY":
                    return new ReadyFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<ReadyFrameData>(dataReader),
                    };
                case "PING":
                    return new PingFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<PingFrameData>(dataReader),
                    };
                case "MESSAGE_CREATE":
                    return new MessageCreateFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<Message>(dataReader),
                    };
                case "TYPING_START":
                    return new TypingFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<TypingFrameData>(dataReader),
                    };
                case "CHANNEL_DELETE":
                    return new ChannelDeleteFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<Channel>(dataReader),
                    };
                case "CHANNEL_UPDATE":
                    return new ChannelUpdateFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<Channel>(dataReader),
                    };
                case "CHANNEL_CREATE":
                    return new ChannelCreateFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<Channel>(dataReader),
                    };
                case "MESSAGE_DELETE":
                    return new MessageDeleteFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<Message>(dataReader),
                    };
                case "MESSAGE_UPDATE":
                    return new MessageUpdateFrame
                    {
                        type = type,
                        opCode = opCode,
                        sequence = sequence,
                        data = serializer.Deserialize<Message>(dataReader),
                    };
                default:
                    return null;
            }
        }
    }
}