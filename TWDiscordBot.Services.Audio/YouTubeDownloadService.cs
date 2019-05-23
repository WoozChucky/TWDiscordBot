using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using TWDiscordBot.Services.Audio.Contracts;
using TWDiscordBot.Services.Audio.Model;

namespace TWDiscordBot.Services.Audio
{
    
    // TODO: Replace Dictionary with IMemoryCache 
    
    public class YouTubeDownloadService : IYouTubeDownloadService
    {
        private readonly IMemoryCache _cache;
        
        public YouTubeDownloadService(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public async Task<DownloadedVideo> DownloadVideo(string url)
        {
            var uri = new Uri(url);
            
            var filename = Uri.UnescapeDataString(HttpUtility.ParseQueryString(uri.Query).Get("v"));

            var cachedVideo = GetFromCache(filename);
            if (cachedVideo != null)
                return cachedVideo;

            var youtubeDl = StartYoutubeDl(
                $"-o songs/{filename}.mp3 --restrict-filenames --extract-audio --no-overwrites --print-json --audio-format mp3 " +
                url);

            if (youtubeDl == null)
            {
                Log.Information("Error: Unable to start process");
                return null;
            }

            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Log.Information($"Download completed with exit code {youtubeDl.ExitCode}");

            var video = JsonConvert.DeserializeObject<DownloadedVideo>(jsonOutput);
            
            AddToCache(video, filename);

            return video;
        }

        public async Task<StreamMetadata> GetLiveStreamData(string url)
        {
            var youtubeDl = StartYoutubeDl("--print-json --skip-download " + url);
            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Log.Information($"Download completed with exit code {youtubeDl.ExitCode}");

            return JsonConvert.DeserializeObject<StreamMetadata>(jsonOutput);
        }
        
        private static Process StartYoutubeDl(string arguments)
        {
            var youtubeDlStartupInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "youtube-dl",
                Arguments = arguments
            };

            Log.Information($"Starting youtube-dl with arguments: {youtubeDlStartupInfo.Arguments}");
            return Process.Start(youtubeDlStartupInfo);
        }

        private DownloadedVideo GetFromCache(string id)
        {
            Log.Information($"Looking for video {id} in cache...");
            
            if (_cache.TryGetValue(id, out var item))
            {
                return item as DownloadedVideo;
            }
            return null;
        }

        private void AddToCache(DownloadedVideo video, string id)
        {
            Log.Information($"Adding video {id} in cache...");
            _cache.Set(id, video, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            });
        }
    }
}