using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model
{
    public class WorldConfiguration
    {
        [JsonProperty("speed")]
        public int Speed { get; set; }
        
        [JsonProperty("unit_speed")]
        public double UnitSpeed { get; set; }
        
        [JsonProperty("moral")]
        public double Moral { get; set; }
        
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

        [JsonProperty("snob")]
        public WorldSnobConfiguration Snob { get; set; }
        
        [JsonProperty("ally")]
        public WorldAllyConfiguration Ally { get; set; }
        
        [JsonProperty("coord")]
        public WorldCoordConfiguration Coords { get; set; }
        
        [JsonProperty("sitter")]
        public WorldSitterConfiguration Sitter { get; set; }
        
        [JsonProperty("sleep")]
        public WorldSleepConfiguration Sleep { get; set; }
    }
}