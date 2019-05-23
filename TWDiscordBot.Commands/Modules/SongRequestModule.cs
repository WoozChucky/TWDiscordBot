using System;
using System.Linq;
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

        [Alias("info", "command", "commands", "?")]
        [Command("help")]
        public async Task DisplayHelp()
        {
            var output = "**[Song Request Commands]**\n\n";
            output += "\t!audio <command> [arguments]\n\n";
            output += "\t!audio `play/request/req/sq/songrequest` <YouTubeURL>\n";
            output += "\t!audio `stream` <YouTubeURL>\n";
            output += "\t!audio `skip/next/nextsong`\n";
            output += "\t!audio `clear`\n";
            output += "\t!audio `list`\n";
            output += "\t!audio `song/nowplaying/np/currentsong/songname`";
            
            await ReplyAsync(output);
        }

        [Alias("sq", "request", "req", "play")]
        [Command("songrequest", RunMode = RunMode.Async)]
        [Summary("Requests a song to be played")]
        public async Task Request([Remainder, Summary("URL of the video to play")] string url)
        {
            await Speedrun(url, 48);
        }
        
        [Command("speedrun", RunMode = RunMode.Async)]
        public async Task Speedrun(string url, int speedModifier)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    await ReplyAsync($"{Context.User.Mention} please provide a valid song URL");
                    return;
                }

                var downloadAnnouncement = await ReplyAsync($"{Context.User.Mention} attempting to fetch {url}");
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
        [Summary("Streams a live stream URL")]
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

        [Command("list")]
        public async Task ListQueue()
        {
            var musics = _songService.ShowQueue().ToList();
            
            if (!musics.Any() && _songService.NowPlaying == null)
            {
                await ReplyAsync("Queue is empty.");
                return;
            }

            var output = string.Join("\n", 
                musics.Select(m => $"-> **{m.Title}** `|{m.DurationString}|` requested by {m.Requester}")
                    .ToArray());
            
            if (_songService.NowPlaying != null)
            {
                var m = _songService.NowPlaying;
                output = output.Insert((output.Length - 1) <= 0 ? 0 : output.Length - 1, 
                    $"-> **{m.Title}** `|{m.DurationString}|` requested by {m.Requester} (NOW PLAYING)\n");
            }
            
            output = output.Insert(0, $"\t`{musics.Count()}` songs in queue.\n\n");
            
            await ReplyAsync(output);
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