using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace TWDiscordBot.Services.TribalWars.Cookies
{
    public class CookiesManager : ICookiesManager
    {
        public async Task CreateOrUpdateCookie(Cookie cookie, BrowserType browser)
        {
            switch (browser)
            {
                case BrowserType.None: 
                    return;
                case BrowserType.GoogleChrome:
                    await CreateOrUpdateChromeCookie(cookie);
                    return;
                default:
                    return;
            }
        }

        public async Task<Cookie> GetCookie(string host, BrowserType type)
        {
            throw new System.NotImplementedException();
        }

        private async Task CreateOrUpdateChromeCookie(Cookie cookie)
        {
            var dbFile = AppDataFolder() + "/Cookies";
            
            using (var connection = new SqliteConnection("" + 
                new SqliteConnectionStringBuilder
                {
                    DataSource = dbFile
                }))
            {
                await connection.OpenAsync();
            }
        }
        
        private static string AppDataFolder()
        {
            var userPath = Environment.GetEnvironmentVariable(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 
                    "LOCALAPPDATA" : "Home");

            
            var path = System.IO.Path.Combine(userPath, "Google/Chrome/User Data/Default");

            return path;
        }
    }
}