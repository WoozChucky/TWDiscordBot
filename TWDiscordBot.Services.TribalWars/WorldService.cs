using System.Threading.Tasks;
using TWDiscordBot.Network.Http.TribalWars.Contracts;
using TWDiscordBot.Network.Http.TribalWars.Model;
using TWDiscordBot.Services.TribalWars.Contracts;

namespace TWDiscordBot.Services.TribalWars
{
    public class WorldService : IWorldService
    {
        private readonly ITribalWarsClient _client;

        public WorldService(ITribalWarsClient client)
        {
            _client = client;
        }
        
        public async Task<WorldConfiguration> GetWorldConfiguration(string world)
        {
            var configuration = await _client.GetWorldConfiguration(world);

            return configuration;
        }
    }
}