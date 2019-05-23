using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ConcurrentQueue<IPlayable> _concurrentQueue;
        private readonly IAudioPlaybackService _audioPlaybackService;
        private bool _isPlaying;

        public IPlayable NowPlaying { get; private set; }

        public SongService(IAudioPlaybackService audioPlaybackService)
        {
            _audioPlaybackService = audioPlaybackService;
            
            _songQueue = new BufferBlock<IPlayable>();
            _concurrentQueue = new ConcurrentQueue<IPlayable>();
            
            _isPlaying = false;
        }

        public IEnumerable<IPlayable> ShowQueue()
        {
            return _concurrentQueue.ToArray().ToList();
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

        public async void Queue(IPlayable video)
        {
            _concurrentQueue.Enqueue(video);
            await _songQueue.SendAsync(video);
        }
        
        private async void ProcessQueue()
        {
            while (await _songQueue.OutputAvailableAsync())
            {
                while (!_isPlaying)
                {
                    Log.Information("Waiting for songs");
                    NowPlaying = await _songQueue.ReceiveAsync();
                    _concurrentQueue.TryDequeue(out _);
                    _isPlaying = true;
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
                    finally
                    {
                        _isPlaying = false;
                    }
                }
            }
        }
    }
}