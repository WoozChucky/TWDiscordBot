using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using TWDiscordBot.Commands.Preconditions;
using TWDiscordBot.Services.Audio.Contracts;

namespace TWDiscordBot.Commands.Modules
{
    [RequireRole("Conselheiros", ErrorMessage = "Not enough permissions.")]
    [Group("admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly ISongService _songService;

        public AdminModule(ISongService songService)
        {
            _songService = songService;
        }

        [Command("register")]
        public async Task RegisterServer(string voiceChannel, string chatChannel)
        {
            var user = Context.User as SocketGuildUser;

            if (user.GuildPermissions.Administrator)
            {
                
            }
            
            var voice = Context.Guild.VoiceChannels.SingleOrDefault(vc => vc.Name.ToLower().Contains(voiceChannel.ToLower()));
            var text = Context.Guild.TextChannels.SingleOrDefault(vc => vc.Name.ToLower().Contains(chatChannel.ToLower()));

            _songService.SetMessageChannel(text);
            _songService.SetVoiceChannel(voice);

            await Context.Channel.SendMessageAsync($"{Context.User.Mention} successfully updated voice server settings.");
        }
    }
}