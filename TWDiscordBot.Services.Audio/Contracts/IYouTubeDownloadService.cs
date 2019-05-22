using System.Threading.Tasks;
using TWDiscordBot.Services.Audio.Model;

namespace TWDiscordBot.Services.Audio.Contracts
{
    public interface IYouTubeDownloadService
    {
        Task<DownloadedVideo> DownloadVideo(string url);
        Task<StreamMetadata> GetLiveStreamData(string url);
    }
}