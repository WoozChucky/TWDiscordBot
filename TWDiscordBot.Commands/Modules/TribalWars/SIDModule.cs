using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TWDiscordBot.Services.TribalWars.Contracts;

namespace TWDiscordBot.Commands.Modules.TribalWars
{
    [Group("sid")]
    public class SIDModule : ModuleBase<SocketCommandContext>
    {
        private readonly ISIDService _sidService;

        public SIDModule(ISIDService sidService)
        {
            _sidService = sidService ?? throw new ArgumentNullException(nameof(sidService));
        }

        [Command("enable")]
        public async Task EnableWatcher()
        {
            await _sidService.ScanForSIDChanges();

            await ReplyAsync("Scanning SID...");
        }

        [Command("disable")]
        public async Task DisableWatcher()
        {
            await _sidService.StopScan();

            await ReplyAsync("Stopped scanning SID...");
        }

        [Command("add")]
        public async Task AddPrivateUser(IGuildUser user)
        {
            await _sidService.AddSecureUser(user);

            await ReplyAsync($"Added {user.Mention} to SID refresher.");
        }

        [Command("get")]
        public async Task DisplaySid()
        {
            var sid = await _sidService.GetCurrentSID();

            await ReplyAsync("SID:");
            await ReplyAsync($"{sid}");
        }
    }
}