using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model
{
    public class WorldNewbieConfiguration
    {
        [JsonProperty("days")]
        public string Days { get; set; }
        
        [JsonProperty("ratio_days")]
        public string RatioDays { get; set; }
        
        [JsonProperty("ratio")]
        public string Ratio { get; set; }
        
        [JsonProperty("removeNewbieVillages")]
        public string RemoveNewbieVillages { get; set; }
    }
}