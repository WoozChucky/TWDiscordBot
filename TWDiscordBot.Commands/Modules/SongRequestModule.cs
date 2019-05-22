using System;
using System.Threading.Tasks;
using Discord.Commands;
using Serilog;
using TWDiscordBot.Services.Audio.Contracts;

namespace TWDiscordBot.Commands.Modules
{
    [Group("audio")]
    public class SongRequestModule : ModuleBase<SocketCommandContext>
    {
        private readonly IYouTubeDownloadService _youTubeDownloadService;
        private readonly ISongService _songService;
        
        public SongRequestModule(IYouTubeDownloadService youTubeDownloadService, ISongService songService)
        {
            _youTubeDownloadService = youTubeDownloadService;
            _songService = songService;
        }

        [Alias("sq", "request", "req", "play")]
        [Command("songrequest", RunMode = RunMode.Async)]
        [Summary("Requests a song to be played")]
        public async Task Request([Remainder, Summary("URL of the video to play")] string url)
        {
            await Speedrun(url, 48);
        }

        [Alias("test")]
        [Command("soundtest", RunMode = RunMode.Async)]
        [Summary("Performs a sound test")]
        public async Task SoundTest()
        {
            await Request("https://www.youtube.com/watch?v=i1GOn7EIbLg");
        }

        [Command("speedrun", RunMode = RunMode.Async)]
        [Summary("Performs a sound test")]
        public async Task Speedrun(string url, int speedModifier)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    await ReplyAsync($"{Context.User.Mention} please provide a valid song URL");
                    return;
                }

                var downloadAnnouncement = await ReplyAsync($"{Context.User.Mention} attempting to download {url}");
                var video = await _youTubeDownloadService.DownloadVideo(url);
                await downloadAnnouncement.DeleteAsync();

                if (video == null)
                {
                    await ReplyAsync($"{Context.User.Mention} unable to queue song, make sure its is a valid supported URL or contact a server admin.");
                    return;
                }

                video.Requester = Context.User.Mention;
                video.Speed = speedModifier;

                await ReplyAsync($"{Context.User.Mention} queued **{video.Title}** | `{TimeSpan.FromSeconds(video.Duration)}` | {url}");

                _songService.Queue(video);
            }
            catch (Exception e)
            {
                Log.Information($"Error while processing song requet: {e}");
            }
        }

        [Command("stream", RunMode = RunMode.Async)]
        [Summary("Streams a livestream URL")]
        public async Task Stream(string url)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    await ReplyAsync($"{Context.User.Mention} please provide a valid URL");
                    return;
                }

                var downloadAnnouncement = await ReplyAsync($"{Context.User.Mention} attempting to open {url}");
                var stream = await _youTubeDownloadService.GetLiveStreamData(url);
                await downloadAnnouncement.DeleteAsync();

                if (stream == null)
                {
                    await ReplyAsync($"{Context.User.Mention} unable to open live stream, make sure its is a valid supported URL or contact a server admin.");
                    return;
                }

                stream.Requester = Context.User.Mention;
                stream.Url = url;

                Log.Information("Attempting to stream {@Stream}", stream);

                await ReplyAsync($"{Context.User.Mention} queued **{stream.Title}** | `{stream.DurationString}` | {url}");

                _songService.Queue(stream);
            }
            catch (Exception e)
            {
                Log.Information($"Error while processing song requet: {e}");
            }
        }

        [Command("clear")]
        [Summary("Clears all songs in queue")]
        public async Task ClearQueue()
        {
            _songService.Clear();
            await ReplyAsync("Queue cleared");
        }

        [Alias("next", "nextsong")]
        [Command("skip")]
        [Summary("Skips current song")]
        public async Task SkipSong()
        {
            _songService.Next();
            await ReplyAsync("Skipped song");
        }

        [Alias("np", "currentsong", "songname", "song")]
        [Command("nowplaying")]
        [Summary("Prints current playing song")]
        public async Task NowPlaying()
        {
            if (_songService.NowPlaying == null)
            {
                await ReplyAsync($"{Context.User.Mention} current queue is empty");
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} now playing `{_songService.NowPlaying.Title}` requested by {_songService.NowPlaying.Requester}");
            }
        }
    }
}