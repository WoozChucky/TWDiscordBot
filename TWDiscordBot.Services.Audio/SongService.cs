using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Discord;
using Serilog;
using TWDiscordBot.Services.Audio.Contracts;

namespace TWDiscordBot.Services.Audio
{
    public class SongService : ISongService
    {
        private IVoiceChannel _voiceChannel;
        private IMessageChannel _messageChannel;
        private readonly BufferBlock<IPlayable> _songQueue;
        private readonly IAudioPlaybackService _audioPlaybackService;

        public IPlayable NowPlaying { get; private set; }

        public SongService(IAudioPlaybackService audioPlaybackService)
        {
            _audioPlaybackService = audioPlaybackService;
            _songQueue = new BufferBlock<IPlayable>();
        }
        
        public void SetVoiceChannel(IVoiceChannel voiceChannel)
        {
            _voiceChannel = voiceChannel;
            ProcessQueue();
        }

        public void SetMessageChannel(IMessageChannel messageChannel)
        {
            _messageChannel = messageChannel;
        }

        public void Next()
        {
            _audioPlaybackService.StopCurrentOperation();
        }

        public IList<IPlayable> Clear()
        {
            _songQueue.TryReceiveAll(out var skippedSongs);

            Log.Information($"Skipped {skippedSongs.Count} songs");

            return skippedSongs;
        }

        public void Queue(IPlayable video)
        {
            _songQueue.Post(video);
        }
        
        private async void ProcessQueue()
        {
            while (await _songQueue.OutputAvailableAsync())
            {
                Log.Information("Waiting for songs");
                NowPlaying = await _songQueue.ReceiveAsync();
                try
                {
                    if (_messageChannel != null)
                    {
                        await _messageChannel?.SendMessageAsync(
                            $"Now playing **{NowPlaying.Title}** | `{NowPlaying.DurationString}` | requested by {NowPlaying.Requester} | {NowPlaying.Url}");
                    }
                        
                    Log.Information("Connecting to voice channel");
                    using (var audioClient = await _voiceChannel.ConnectAsync())
                    {
                        Log.Information("Connected!");
                        await _audioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Speed);
                    }

                    NowPlaying.OnPostPlay();
                }
                catch (Exception e)
                {
                    Log.Information($"Error while playing song: {e}");
                }
            }
        }
    }
}