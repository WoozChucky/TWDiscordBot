using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model
{
    public class WorldSitterConfiguration
    {
        [JsonProperty("allow")]
        public int Allow { get; set; }
    }
}