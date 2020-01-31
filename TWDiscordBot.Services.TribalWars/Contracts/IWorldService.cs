using System.Collections.Generic;
using System.Threading.Tasks;
using TWDiscordBot.Network.Http.TribalWars.Model.Configuration;
using TWDiscordBot.Network.Http.TribalWars.Model.World;

namespace TWDiscordBot.Services.TribalWars.Contracts
{
    public interface IWorldService
    {
        Task<WorldConfiguration> GetWorldConfiguration(string world);
        Task<IEnumerable<WorldPlayer>> GetWorldPlayers(string world);
        Task<WorldPlayer> GetWorldPlayer(string world, string playerName);
        Task<WorldPlayer> GetWorldPlayer(string world, long playerId);
    }
}