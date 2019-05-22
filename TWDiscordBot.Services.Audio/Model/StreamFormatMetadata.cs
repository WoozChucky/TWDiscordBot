using Newtonsoft.Json;

namespace TWDiscordBot.Services.Audio.Model
{
    public class StreamFormatMetadata
    {
        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "acodec")]
        public string Codec { get; set; }
    }
}