using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldSitterConfiguration
    {
        [JsonProperty("allow")]
        public int Allow { get; set; }
    }
}