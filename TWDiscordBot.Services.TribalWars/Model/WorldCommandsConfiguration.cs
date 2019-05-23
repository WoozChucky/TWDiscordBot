using Newtonsoft.Json;

namespace TWDiscordBot.Services.TribalWars.Model
{
    public class WorldCommandsConfiguration
    {
        [JsonProperty("millis_arrival")]
        public string MillisArrival { get; set; }
        
        [JsonProperty("command_cancel_time")]
        public string CommandCancelTime { get; set; }
    }
}