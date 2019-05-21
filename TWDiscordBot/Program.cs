using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWDiscordBot.Background;
using TWDiscordBot.Workers;
using TWDiscordBot.Workers.Contracts;

namespace TWDiscordBot
{
    public class Program
    {
        private readonly DiscordSocketClient _client;

        private readonly CommandService _commands;

        private readonly IServiceProvider _serviceProvider;


        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _client = new DiscordSocketClient();

            _commands = new CommandService();

            _serviceProvider = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<CommandHandler>()
                .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
                .AddHostedService<QueuedHostedService>()
                .BuildServiceProvider();
        }

        public async Task MainAsync()
        {
            await _serviceProvider.GetService<CommandHandler>().InitializeAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
                    services.AddHostedService<Worker>();
                    services.AddHostedService<QueuedHostedService>();
                });
    }
}