using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TWDiscordBot.Commands.Modules;

namespace TWDiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services, CommandService commands, DiscordSocketClient client)
        {
            _commands = commands;
            _services = services;
            _client = client;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.MessageDeleted += MessageDelete;
            _client.MessageUpdated += MessageUpdated;
            _client.MessagesBulkDeleted += MessageBulkUpdated;
            _client.Ready += BotReady;
            _client.Log += Log;

            //await _commands.AddModulesAsync(Assembly.GetAssembly(typeof(InfoModule)), _services);

            await _commands.AddModuleAsync<InfoModule>(_services);

            await _client.LoginAsync(TokenType.Bot, "NTgwMzgzNDY2MTE0MzgzOTA2.XOP_dQ.isBpaeCcetdp9Rs1YuiQT7BFws8");
            await _client.StartAsync();
        }

        private async Task MessageDelete(Cacheable<IMessage, ulong> deleted, ISocketMessageChannel channel)
        {
            var message = await deleted.GetOrDownloadAsync();
            Console.WriteLine($"Message Deleted: {message}");
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine($"Message Updated: {message} -> {after}");
        }

        private async Task MessageBulkUpdated(IReadOnlyCollection<Cacheable<IMessage, ulong>> messages, ISocketMessageChannel channel)
        {
            foreach (var before in messages)
            {
                var message = await before.GetOrDownloadAsync();
                Console.WriteLine($"Bulk Updated: {message}");
            }
        }

        private Task BotReady()
        {
            Console.WriteLine("TWBot is connected!");
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            // Don't process the command if it was a system message
            var message = socketMessage as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            var argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            var result = await _commands.ExecuteAsync(context, argPos, null);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
