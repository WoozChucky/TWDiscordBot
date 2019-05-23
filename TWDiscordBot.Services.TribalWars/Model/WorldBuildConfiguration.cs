using Newtonsoft.Json;

namespace TWDiscordBot.Services.TribalWars.Model
{
    public class WorldBuildConfiguration
    {
        [JsonProperty("destroy")]
        public string Destroy { get; set; }
    }
}