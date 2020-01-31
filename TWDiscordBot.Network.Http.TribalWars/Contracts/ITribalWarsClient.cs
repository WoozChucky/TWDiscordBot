using System.Collections.Generic;
using System.Threading.Tasks;
using TWDiscordBot.Network.Http.Contracts;
using TWDiscordBot.Network.Http.TribalWars.Model;
using TWDiscordBot.Network.Http.TribalWars.Model.Configuration;
using TWDiscordBot.Network.Http.TribalWars.Model.World;

namespace TWDiscordBot.Network.Http.TribalWars.Contracts
{
    public interface ITribalWarsClient : IRestClient
    {
        Task<WorldConfiguration> GetWorldConfiguration(string world);
        Task<IEnumerable<WorldPlayer>> GetWorldPlayers(string world);
    }
}