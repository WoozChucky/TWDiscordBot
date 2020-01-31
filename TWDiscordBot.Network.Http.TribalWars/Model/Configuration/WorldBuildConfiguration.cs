using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldBuildConfiguration
    {
        [JsonProperty("destroy")]
        public int Destroy { get; set; }
    }
}