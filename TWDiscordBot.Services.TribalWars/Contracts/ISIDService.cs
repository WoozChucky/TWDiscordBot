using System.Threading.Tasks;
using Discord;

namespace TWDiscordBot.Services.TribalWars.Contracts
{
    public interface ISIDService
    {
        Task SetMessageChannel(IMessageChannel messageChannel);
        Task AddSecureUser(IGuildUser user);
        Task ScanForSIDChanges();
        Task<string> GetCurrentSID();
        Task StopScan();
    }
}