using Newtonsoft.Json;

namespace TWDiscordBot.Services.TribalWars.Model
{
    public class WorldConfiguration
    {
        [JsonProperty("speed")]
        public string Speed { get; set; }
        
        [JsonProperty("unit_speed")]
        public string UnitSpeed { get; set; }
        
        public string Moral { get; set; }
        
        public WorldBuildConfiguration Build { get; set; }
    }
}