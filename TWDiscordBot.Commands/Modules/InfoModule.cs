using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace TWDiscordBot.Commands.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        [Summary("Tells information about the bot.")]
        public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        {
            var like = new Emoji("\uD83D\uDC4C");
            await ReplyAsync($"{echo} {like}");
        }
    }
}
