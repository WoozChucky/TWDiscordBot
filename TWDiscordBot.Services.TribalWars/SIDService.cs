using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using TWDiscordBot.Services.TribalWars.Contracts;
using TWDiscordBot.Services.TribalWars.Cookies;
using TWDiscordBot.Services.TribalWars.Scanner;

namespace TWDiscordBot.Services.TribalWars
{
    public class SIDService : ISIDService
    {
        private IMessageChannel _messageChannel;

        private readonly List<IGuildUser> _users;

        private Cookie _sid;

        private readonly ISIDScanner _scanner;

        private readonly ICookiesManager _cookieManager;

        public SIDService(ISIDScanner scanner, ICookiesManager cookieManager)
        {
            _users = new List<IGuildUser>();
            _scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
            _cookieManager = cookieManager;
            _scanner.NewSID += ScannerOnNewSID;
        }

        private async void ScannerOnNewSID(object sender, Cookie e)
        {
            if (_sid == null || _sid.Value != e.Value)
            {
                _sid = e;

                BroadcastSID();

                try
                {
                    await _cookieManager.CreateOrUpdateCookie(e, BrowserType.GoogleChrome);
                }
                catch (Exception exception)
                {
                    Serilog.Log.Warning("Error occurred while updating browser cookie.\n{error}", exception.Message);
                }
            }
        }

        private async void BroadcastSID()
        {
            await _messageChannel.SendMessageAsync($"New SID!!\n {_sid.Value}");

            foreach (var guildUser in _users)
            {
                await guildUser.SendMessageAsync(_sid.Value);
            }
        }

        public Task SetMessageChannel(IMessageChannel messageChannel)
        {
            _messageChannel = messageChannel ?? throw new ArgumentNullException(nameof(messageChannel));
            return Task.CompletedTask;
        }

        public Task AddSecureUser(IGuildUser user)
        {
            if (_users.All(u => u.Id != user.Id))
            {
                _users.Add(user);
            }
            return Task.CompletedTask;
        }

        public async Task ScanForSIDChanges()
        {
            await _scanner.StartScan();
        }

        public async Task StopScan()
        {
            await _scanner.StopScan();
        }

        public async Task<string> GetCurrentSID()
        {
            if (_sid == null || string.IsNullOrEmpty(_sid.Value))
            {
                // TODO(Levezinho): Implement this
                
                var sid = await _cookieManager.GetCookie("pt70", "sid", BrowserType.GoogleChrome);
                
                return sid.Value;
            }
            else
            {
                return await Task.FromResult(_sid?.Value);
            }
        }

        
    }
}