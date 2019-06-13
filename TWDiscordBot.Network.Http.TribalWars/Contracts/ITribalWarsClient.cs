using System.Threading.Tasks;
using TWDiscordBot.Network.Http.Contracts;
using TWDiscordBot.Network.Http.TribalWars.Model;

namespace TWDiscordBot.Network.Http.TribalWars.Contracts
{
    public interface ITribalWarsClient : IRestClient
    {
        Task<WorldConfiguration> GetWorldConfiguration(string world);
    }
}