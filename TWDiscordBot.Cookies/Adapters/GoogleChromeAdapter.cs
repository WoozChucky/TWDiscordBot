using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace TWDiscordBot.Cookies.Adapters
{
    public class GoogleChromeAdapter : ICookieAdapter
    {
        public async Task<string> GetCookie(string domain, string name)
        {
            var dbFile = ChromeAppDataFolder() + "/Cookies";
            
            var dbSid = string.Empty;
            
            using (var connection = new SqliteConnection("" + 
                                                         new SqliteConnectionStringBuilder
                                                         {
                                                             DataSource = dbFile
                                                         }))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();

                command.CommandText =
                    "SELECT encrypted_value " +
                    "FROM cookies " +
                    $"WHERE host_key LIKE '%{domain}%' AND name LIKE '%{name}%'";

                var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var encryptedData = (byte[]) reader[0];
                    var decodedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                    dbSid = Encoding.ASCII.GetString(decodedData);
                }
                
                connection.Close();
            }

            return dbSid;
        }

        public Task UpdateCookie(string domain, string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateCookie(string domain, string name, string value)
        {
            throw new System.NotImplementedException();
        }
        
        private static string ChromeAppDataFolder()
        {
            var userPath = Environment.GetEnvironmentVariable(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 
                    "LOCALAPPDATA" : "Home");

            
            var path = System.IO.Path.Combine(userPath, "Google/Chrome/User Data/Default");

            return path;
        }
    }
}