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

            await Context.Channel.SendMessageAsync($"{world} world info:");
            await Context.Channel.SendMessageAsync(JsonConvert.SerializeObject(configuration, Formatting.Indented));
        }

        /*
         * Ataque
         * lgAMAw0DDgMNAwwDDQQMBA4EAAUBBQ8KDAUNBQ4FCQEQCgAFAQUIBQIFCgEQBQAFEAMMAw0DDgMMAg0CDgUIBQ8FAgUQAggFAwUPBQAFDwIIBQsKBwEPAwEFAgUQAw8BDAMNAw4DEAMLCg8DAgUMAg0CDgIBBRACDAINAg4CDwEBAwwCDgINAgECDAENAQ4BCwISFAMKEAIA9ICAgEF0YXF1ZfSAgIAz
         *
         * Defesa
         * jAAMAw0DDgMNAwwDDQQMBA4EAAUBBQ8KDAUNBQ4FEAoABQEFCAUCBQoBEAMMAw0DDgMQAgwCDQIOAgkBCAUPBQIFAAUPBQAFEAUBBQMFDwMLCggFDAMNAw4DDwIIBQcBDAINAg4CDgMCBRAFDwMLCgEFEhQPAgEDDAMNAw4DAQICBQwCDQIOAhAFCwIDCgD0gICARGVmZXNh9ICAgDM=
         *
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