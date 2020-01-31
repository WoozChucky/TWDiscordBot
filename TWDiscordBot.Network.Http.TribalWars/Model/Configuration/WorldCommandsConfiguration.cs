using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldCommandsConfiguration
    {
        [JsonProperty("millis_arrival")]
        public int MillisArrival { get; set; }
        
        [JsonProperty("command_cancel_time")]
        public int CommandCancelTime { get; set; }
    }
}