using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using TWDiscordBot.Services.TribalWars.Cookies;

namespace TWDiscordBot.Services.TribalWars.Scanner
{
    public class SIDScanner : ISIDScanner
    {
        public event EventHandler<Cookie> NewSID;

        private readonly FileSystemWatcher _watcher;
        
        private static readonly object Lock = new object();
        
        DateTime lastRead = DateTime.MinValue;

        private bool _processing;

        public SIDScanner()
        {
            _watcher =
                new FileSystemWatcher("D:/", "global.json")
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };
            _watcher.Changed += WatcherOnChanged;
            _watcher.Created += WatcherOnCreated;
            _watcher.Deleted += WatcherOnDeleted;
        }
        
        public Task StartScan()
        {
            _watcher.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }

        public Task StopScan()
        {
            _watcher.EnableRaisingEvents = false;
            return Task.CompletedTask;
        }
        
        private async void RefreshCookie(string filePath)
        {
            await Task.Delay(5000);

            var fileContent = await File.ReadAllTextAsync(filePath);

            /*
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(fileContent);

            var json = JsonConvert.SerializeXmlNode(xmlDoc);

            var cookieFile = JsonConvert.DeserializeObject<CookieFile>(json);

            var cookie = cookieFile.ArrayOfCookie.Cookie.FirstOrDefault(c => c.Name == "sid");
            */

            var cookie = new Cookie
            {
                Domain = "pt70.tribalwars.com.pt",
                Name = "sid",
                Value = fileContent
            };

            if (cookie != null)
            {
                OnNewSID(cookie);
            }
        }
        
        protected virtual void OnNewSID(Cookie e)
        {
            var handler = NewSID;
            handler?.Invoke(this, e);
        }
        
        #region FileWatcher Events

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
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

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
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

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            lock (Lock)
            {
                if (!_processing)
                {
                    _processing = true;
                    var lastWriteTime = File.GetLastWriteTime(e.FullPath);
                    if (lastWriteTime.Ticks != lastRead.Ticks)
                    {
                        RefreshCookie(e.FullPath);
                        lastRead = lastWriteTime;
                    }
                    _processing = false;
                }
            }
        }

        #endregion
    }
}