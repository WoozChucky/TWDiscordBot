using System.Threading.Tasks;
using TWDiscordBot.Network.Http.TribalWars.Model;

namespace TWDiscordBot.Services.TribalWars.Contracts
{
    public interface IWorldService
    {
        Task<WorldConfiguration> GetWorldConfiguration(string world);
    }
}