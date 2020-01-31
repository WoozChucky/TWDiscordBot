using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldGameConfiguration
    {
        [JsonProperty("buildtime_formula")]
        public int BuildTimeFormula { get; set; }
        
        [JsonProperty("knight")]
        public int Knight { get; set; }
        
        [JsonProperty("knight_new_items")]
        public string KnightNewItems { get; set; }
        
        [JsonProperty("archer")]
        public int Archer { get; set; }
        
        [JsonProperty("tech")]
        public int Tech { get; set; }
        
        [JsonProperty("farm_limit")]
        public int FarmLimit { get; set; }
        
        [JsonProperty("church")]
        public int Church { get; set; }
        
        [JsonProperty("watchtower")]
        public int Watchtower { get; set; }
        
        [JsonProperty("stronghold")]
        public int Stronghold { get; set; }
        
        [JsonProperty("fake_limit")]
        public int FakeLimit { get; set; }
        
        [JsonProperty("barbarian_rise")]
        public double BarbarianRise { get; set; }
        
        [JsonProperty("barbarian_shrink")]
        public double BarbarianShrink { get; set; }
        
        [JsonProperty("barbarian_max_points")]
        public int BarbarianMaxPoints { get; set; }
        
        [JsonProperty("hauls")]
        public int Hauls { get; set; }
        
        [JsonProperty("hauls_base")]
        public int HaulsBase { get; set; }
        
        [JsonProperty("hauls_max")]
        public int HaulsMax { get; set; }
        
        [JsonProperty("base_production")]
        public int BaseProduction { get; set; }
        
        [JsonProperty("event")]
        public int Event { get; set; }
    }
}