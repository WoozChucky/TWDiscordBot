using Newtonsoft.Json;

namespace TWDiscordBot.Services.TribalWars.Model
{
    public class WorldGameConfiguration
    {
        [JsonProperty("buildtime_formula")]
        public string BuildTimeFormula { get; set; }
        
        [JsonProperty("knight")]
        public string Knight { get; set; }
        
        [JsonProperty("knight_new_items")]
        public string KnightNewItems { get; set; }
        
        [JsonProperty("archer")]
        public string Archer { get; set; }
        
        [JsonProperty("tech")]
        public string Tech { get; set; }
        
        [JsonProperty("farm_limit")]
        public string FarmLimit { get; set; }
        
        [JsonProperty("church")]
        public string Church { get; set; }
        
        [JsonProperty("watchtower")]
        public string Watchtower { get; set; }
        
        [JsonProperty("stronghold")]
        public string Stronghold { get; set; }
        
        [JsonProperty("fake_limit")]
        public string FakeLimit { get; set; }
        
        [JsonProperty("barbarian_rise")]
        public string BarbarianRise { get; set; }
        
        [JsonProperty("barbarian_shrink")]
        public string BarbarianShrink { get; set; }
        
        [JsonProperty("barbarian_max_points")]
        public string BarbarianMaxPoints { get; set; }
        
        [JsonProperty("hauls")]
        public string Hauls { get; set; }
        
        [JsonProperty("hauls_base")]
        public string HaulsBase { get; set; }
        
        [JsonProperty("hauls_max")]
        public string HaulsMax { get; set; }
        
        [JsonProperty("base_production")]
        public string BaseProduction { get; set; }
        
        [JsonProperty("event")]
        public string Event { get; set; }
        
        [JsonProperty("suppress_events")]
        public string SuppressEvents { get; set; }
        
        [JsonProperty("compete_event")]
        public string CompeteEvent { get; set; }
    }
}