using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldSnobConfiguration
    {
        [JsonProperty("gold")]
        public int Gold { get; set; }
        
        [JsonProperty("cheap_rebuild")]
        public int CheapRebuild { get; set; }
        
        [JsonProperty("rise")]
        public int Rise { get; set; }
        
        [JsonProperty("max_dist")]
        public int MaxDistance { get; set; }
        
        [JsonProperty("factor")]
        public int Factor { get; set; }
        
        [JsonProperty("coin_wood")]
        public int CoinWood { get; set; }
        
        [JsonProperty("coin_stone")]
        public int CoinStone { get; set; }
        
        [JsonProperty("coin_iron")]
        public int CoinIron { get; set; }
        
    }
}