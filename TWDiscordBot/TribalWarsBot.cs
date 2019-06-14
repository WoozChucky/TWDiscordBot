using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TWDiscordBot.Commands.Modules;
using TWDiscordBot.Commands.Modules.TribalWars;
using TWDiscordBot.Services.Audio.Contracts;
using TWDiscordBot.Services.TribalWars.Contracts;

namespace TWDiscordBot
{
    public class TribalWarsBot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public TribalWarsBot(IServiceProvider services, CommandService commands, DiscordSocketClient client)
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

            _commands.CommandExecuted += OnCommandExecutedAsync;
            //await _commands.AddModulesAsync(Assembly.GetAssembly(typeof(InfoModule)), _services);

            await _commands.AddModuleAsync<InfoModule>(_services);
            await _commands.AddModuleAsync<SongRequestModule>(_services);
            await _commands.AddModuleAsync<AdminModule>(_services);
            await _commands.AddModuleAsync<WorldConfigModule>(_services);
            await _commands.AddModuleAsync<SIDModule>(_services);

            await _client.LoginAsync(TokenType.Bot, "NTgwMzgzNDY2MTE0MzgzOTA2.XOWMgg.Z9gFFyixqDPZMVtJW9-zvOHdDwI");
            await _client.StartAsync();

            _client.GuildAvailable += OnClientGuildAvailable;
        }

        private Task OnClientGuildAvailable(SocketGuild guild)
        {
            if (guild.Name == "Lole")
            {
                Serilog.Log.Information("Registering handler for {guild}", guild.Name);
                var musicVoiceChannel = guild.VoiceChannels.SingleOrDefault(t => t.Name.ToLower().Contains("general"));
                var musicRequestChannel = guild.TextChannels.SingleOrDefault(t => t.Name.ToLower().Contains("general"));

                _services.GetService<ISongService>().SetVoiceChannel(musicVoiceChannel);
                _services.GetService<ISongService>().SetMessageChannel(musicRequestChannel);
                _services.GetService<ISIDService>().SetMessageChannel(musicRequestChannel);
            }

            Serilog.Log.Information("Discovered server {guild}", guild.Name);
            return Task.CompletedTask;
        }

        private async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        private async Task MessageDelete(Cacheable<IMessage, ulong> deleted, ISocketMessageChannel channel)
        {
            var message = await deleted.GetOrDownloadAsync();
            Serilog.Log.Information("Message Deleted: {message}", message);
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var message = await before.GetOrDownloadAsync();
            Serilog.Log.Information("Message Updated: {message} -> {after}", message, after);
        }

        private async Task MessageBulkUpdated(IReadOnlyCollection<Cacheable<IMessage, ulong>> messages, ISocketMessageChannel channel)
        {
            foreach (var before in messages)
            {
                var message = await before.GetOrDownloadAsync();
                Serilog.Log.Information("Bulk Updated: {message}", message);
            }
        }

        private Task BotReady()
        {
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            // Don't process the command if it was a system message
            if (!(socketMessage is SocketUserMessage message)) return;

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
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        private Task Log(LogMessage msg)
        {
            Serilog.Log.Information("{msg}", msg);
            return Task.CompletedTask;
        }
    }
}
