using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model
{
    public class WorldBuildConfiguration
    {
        [JsonProperty("destroy")]
        public int Destroy { get; set; }
    }
}