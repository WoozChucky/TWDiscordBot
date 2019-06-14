using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Discord;
using Newtonsoft.Json;
using TWDiscordBot.Services.TribalWars.Contracts;
using TWDiscordBot.Services.TribalWars.Model;

namespace TWDiscordBot.Services.TribalWars
{
    public class SIDService : ISIDService
    {
        private IMessageChannel _messageChannel;

        private readonly List<IGuildUser> _users;

        private readonly FileSystemWatcher _watcher;

        private string _sid = string.Empty;

        private static readonly object Lock = new object();

        private bool _processing;

        public SIDService()
        {
            _users = new List<IGuildUser>();
            _watcher =
                new FileSystemWatcher("C:/Users/nunol/OneDrive/Ambiente de Trabalho/M67", "cookies.txt")
                {
                    NotifyFilter = NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };
            _watcher.Changed += WatcherOnChanged;
            _watcher.Created += WatcherOnCreated;
            _watcher.Deleted += WatcherOnDeleted;
        }

        public Task SetMessageChannel(IMessageChannel messageChannel)
        {
            _messageChannel = messageChannel ?? throw new ArgumentNullException(nameof(messageChannel));
            return Task.CompletedTask;
        }

        public Task AddSecureUser(IGuildUser user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task ScanForSIDChanges()
        {
            _watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        public async Task<string> GetCurrentSID()
        {
            return await Task.FromResult(_sid);
        }

        public Task StopScan()
        {
            _watcher.EnableRaisingEvents = false;

            return Task.CompletedTask;
        }

        #region FileWatcher Events

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            //RefreshCookie(e.FullPath);
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            //RefreshCookie(e.FullPath);
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            lock (Lock)
            {
                if (!_processing)
                {
                    _processing = true;
                    RefreshCookie(e.FullPath);
                    _processing = false;
                }
            }
        }

        private async void RefreshCookie(string filePath)
        {
            await Task.Delay(5000);

            var fileContent = await File.ReadAllTextAsync(filePath);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(fileContent);

            var json = JsonConvert.SerializeXmlNode(xmlDoc);

            var cookieFile = JsonConvert.DeserializeObject<CookieFile>(json);

            var cookie = cookieFile.ArrayOfCookie.Cookie.FirstOrDefault(c => c.Name == "sid");

            if (cookie != null && _sid != cookie.Value)
            {
                _sid = cookie.Value;

                await _messageChannel.SendMessageAsync($"New SID!!\n {_sid}");

                foreach (var guildUser in _users)
                {
                    await guildUser.SendMessageAsync(_sid);
                }
            }
        }

        #endregion
    }
}