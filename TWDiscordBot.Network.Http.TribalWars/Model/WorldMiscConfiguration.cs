using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model
{
    public class WorldMiscConfiguration
    {
        [JsonProperty("kill_ranking")]
        public string KillRanking { get; set; }
        
        [JsonProperty("tutorial")]
        public string Tutorial { get; set; }
        
        [JsonProperty("trade_cancel_time")]
        public string TradeCancelTime { get; set; }
    }
}