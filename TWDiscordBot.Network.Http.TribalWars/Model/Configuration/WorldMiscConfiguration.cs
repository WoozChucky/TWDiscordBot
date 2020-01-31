using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldMiscConfiguration
    {
        [JsonProperty("kill_ranking")]
        public int KillRanking { get; set; }
        
        [JsonProperty("tutorial")]
        public int Tutorial { get; set; }
        
        [JsonProperty("trade_cancel_time")]
        public int TradeCancelTime { get; set; }
    }
}