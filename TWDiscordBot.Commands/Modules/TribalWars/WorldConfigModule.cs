using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json;
using TWDiscordBot.Services.TribalWars.Contracts;

namespace TWDiscordBot.Commands.Modules.TribalWars
{
    [Group("tw")]
    public class WorldConfigModule : ModuleBase<SocketCommandContext>
    {
        private readonly IWorldService _worldService;

        public WorldConfigModule(IWorldService worldService)
        {
            _worldService = worldService;
        }

        [Command("info")]
        public async Task DisplayInfo(string world)
        {
            var configuration = await _worldService.GetWorldConfiguration(world);

            await ReplyAsync($"{world} world info:");
            await ReplyAsync(JsonConvert.SerializeObject(configuration, Formatting.Indented));
        }
        
        [Command("players"), Alias("ps")]
        public async Task DisplayPlayers(string world)
        {
            await _worldService.GetWorldPlayers(world);

            await ReplyAsync("Refresh player data for " + world + ".");
        }

        [Command("player"), Alias("p")]
        public async Task DisplayPlayer(string world, string playerName)
        {
            var player = await _worldService.GetWorldPlayer(world, playerName);

            await ReplyAsync(JsonConvert.SerializeObject(player, Formatting.Indented));
        }
        
        [Command("player"), Alias("p")]
        public async Task DisplayPlayer(string world, long playerId)
        {
            var player = await _worldService.GetWorldPlayer(world, playerId);

            await ReplyAsync(JsonConvert.SerializeObject(player, Formatting.Indented));
        }

        /*
         * https://forum.tribalwars.us/index.php?threads/world-data.5996/
         *
         * https://pt67.tribalwars.com.pt/interface.php?func=get_config
         *
         * https://pt67.tribalwars.com.pt/interface.php?func=get_building_info
         *
         * https://pt67.tribalwars.com.pt/interface.php?func=get_unit_info
         */
    }
}