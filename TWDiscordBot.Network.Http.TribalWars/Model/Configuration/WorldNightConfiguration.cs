using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldNightConfiguration
    {
        [JsonProperty("active")]
        public int Active { get; set; }
        
        [JsonProperty("start_hour")]
        public int StartHour { get; set; }
        
        [JsonProperty("end_hour")]
        public int EndHour { get; set; }
        
        [JsonProperty("def_factor")]
        public int DefenseFactor { get; set; }
    }
}