using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model
{
    public class WorldNewbieConfiguration
    {
        [JsonProperty("days")]
        public int Days { get; set; }
        
        [JsonProperty("ratio_days")]
        public int RatioDays { get; set; }
        
        [JsonProperty("ratio")]
        public int Ratio { get; set; }
        
        [JsonProperty("removeNewbieVillages")]
        public int RemoveNewbieVillages { get; set; }
    }
}