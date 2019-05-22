using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;

namespace TWDiscordBot.Services.Audio.Contracts
{
    public interface ISongService
    {
        IPlayable NowPlaying { get; }

        void SetVoiceChannel(IVoiceChannel voiceChannel);
        
        void SetMessageChannel(IMessageChannel messageChannel);

        void Next();
        
        IList<IPlayable> Clear();
        
        void Queue(IPlayable video);
    }
}