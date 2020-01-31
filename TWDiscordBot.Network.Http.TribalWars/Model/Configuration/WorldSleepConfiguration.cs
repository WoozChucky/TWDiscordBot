using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldSleepConfiguration
    {
        [JsonProperty("active")]
        public int Active { get; set; }
        
        [JsonProperty("delay")]
        public int Delay { get; set; }
        
        [JsonProperty("min")]
        public int Min { get; set; }
        
        [JsonProperty("max")]
        public int Max { get; set; }
        
        [JsonProperty("min_awake")]
        public int MinAwake { get; set; }
        
        [JsonProperty("max_awake")]
        public int MaxAwake { get; set; }
        
        [JsonProperty("warn_time")]
        public int WarnTime { get; set; }
    }
}