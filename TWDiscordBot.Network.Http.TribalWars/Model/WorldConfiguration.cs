using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model
{
    public class WorldConfiguration
    {
        [JsonProperty("speed")]
        public string Speed { get; set; }
        
        [JsonProperty("unit_speed")]
        public string UnitSpeed { get; set; }
        
        [JsonProperty("moral")]
        public string Moral { get; set; }
        
        [JsonProperty("build")]
        public WorldBuildConfiguration Build { get; set; }

        [JsonProperty("misc")]
        public WorldMiscConfiguration Misc { get; set; }

        [JsonProperty("commands")]
        public WorldCommandsConfiguration Commands { get; set; }
        
        [JsonProperty("newbie")]
        public WorldNewbieConfiguration Newbie { get; set; }

        [JsonProperty("game")]
        public WorldGameConfiguration Game { get; set; }
    }
}