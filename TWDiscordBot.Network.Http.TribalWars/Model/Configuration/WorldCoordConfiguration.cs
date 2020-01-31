using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldCoordConfiguration
    {
        [JsonProperty("map_size")]
        public int MapSize { get; set; }
        
        [JsonProperty("func")]
        public int Func { get; set; }
        
        [JsonProperty("empty_villages")]
        public int EmptyVillages { get; set; }
        
        [JsonProperty("bonus_villages")]
        public int BonusVillages { get; set; }
        
        [JsonProperty("bonus_new")]
        public int BonusNew { get; set; }
        
        [JsonProperty("inner")]
        public int Inner { get; set; }
        
        [JsonProperty("select_start")]
        public int SelectStart { get; set; }
        
        [JsonProperty("village_move_wait")]
        public int VillageMoveWait { get; set; }
        
        [JsonProperty("noble_restart")]
        public int NobleRestart { get; set; }
        
        [JsonProperty("start_villages")]
        public int StartVillages { get; set; }
    }
}