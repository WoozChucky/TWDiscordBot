using System.Threading.Tasks;
using Discord.Audio;

namespace TWDiscordBot.Services.Audio.Contracts
{
    public interface IAudioPlaybackService
    {
        Task SendAsync(IAudioClient client, string path, int speedModifier);

        void StopCurrentOperation();
    }
}