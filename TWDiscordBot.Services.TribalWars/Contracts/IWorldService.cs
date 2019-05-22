using System.Threading.Tasks;
using TWDiscordBot.Services.TribalWars.Model;

namespace TWDiscordBot.Services.TribalWars.Contracts
{
    public interface IWorldService
    {
        Task<WorldConfiguration> GetWorldConfiguration(string world);
    }
}