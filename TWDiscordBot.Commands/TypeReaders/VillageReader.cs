using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace TWDiscordBot.Commands.TypeReaders
{
    public class VillageReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            throw new NotImplementedException();
        }
    }
}