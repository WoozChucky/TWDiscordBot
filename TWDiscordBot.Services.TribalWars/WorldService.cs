using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TWDiscordBot.Network.Http.TribalWars.Contracts;
using TWDiscordBot.Network.Http.TribalWars.Model.Configuration;
using TWDiscordBot.Network.Http.TribalWars.Model.World;
using TWDiscordBot.Services.TribalWars.Contracts;

namespace TWDiscordBot.Services.TribalWars
{
    public class WorldService : IWorldService
    {
        private readonly ITribalWarsClient _client;

        private IEnumerable<WorldPlayer> _players;

        public WorldService(ITribalWarsClient client)
        {
            _client = client;
            
            _players = _client.GetWorldPlayers("pt70").Result;
        }
        
        public async Task<WorldConfiguration> GetWorldConfiguration(string world)
        {
            var configuration = await _client.GetWorldConfiguration(world);

            return configuration;
        }

        public async Task<IEnumerable<WorldPlayer>> GetWorldPlayers(string world)
        {
            _players = await _client.GetWorldPlayers(world);

            return _players;
        }

        public Task<WorldPlayer> GetWorldPlayer(string world, string playerName)
        {
            var player = _players.FirstOrDefault(p => p.Name.ToLower().Contains(playerName.ToLower()));
            
            return Task.FromResult(player);
        }

        public Task<WorldPlayer> GetWorldPlayer(string world, long playerId)
        {
            var player = _players.FirstOrDefault(p => p.Id == playerId);
            
            return Task.FromResult(player);
        }
    }
}