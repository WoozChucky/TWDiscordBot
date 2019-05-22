using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TWDiscordBot.Services.Audio;
using TWDiscordBot.Services.Audio.Contracts;
using TWDiscordBot.Services.Threading;
using TWDiscordBot.Services.Threading.Contracts;
using TWDiscordBot.Services.TribalWars;
using TWDiscordBot.Services.TribalWars.Contracts;

namespace TWDiscordBot
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //.WriteTo.Async(a => a.RollingFile("Logs/log-{Date}.txt"))
                .WriteTo.Console()
                .CreateLogger();
            
            DependencyHelper.TestDependencies();

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private Program()
        {
            var client = new DiscordSocketClient();

            var commands = new CommandService();

            _serviceProvider = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton<TribalWarsBot>()
                .AddSingleton<ISongService, SongService>()
                .AddSingleton<IWorldService, WorldService>()
                .AddSingleton<IYouTubeDownloadService, YouTubeDownloadService>()
                .AddSingleton<IAudioPlaybackService, AudioPlaybackService>()
                .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
                .AddHostedService<QueuedHostedService>()
                .BuildServiceProvider();
        }

        private async Task MainAsync()
        {
            await _serviceProvider.GetService<TribalWarsBot>().InitializeAsync();

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