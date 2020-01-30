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
        private readonly string _databaseFile = ChromeAppDataFolder() + "/Cookies";
        
        public async Task<string> GetCookie(string domain, string name)
        {
            var dbSid = string.Empty;

            await using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder
                                                         {
                                                             DataSource = _databaseFile,
                                                             Mode = SqliteOpenMode.ReadWrite
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

        public async Task UpdateCookie(string domain, string name, string value)
        {
            await using var connection = new SqliteConnection("" +
                                                              new SqliteConnectionStringBuilder
                                                              {
                                                                  DataSource = _databaseFile,
                                                                  Mode = SqliteOpenMode.ReadWrite
                                                              });
            await connection.OpenAsync();

            var command = connection.CreateCommand();

            command.CommandText =
                "UPDATE cookies " +
                "SET encrypted_value = (@sid) " +
                $"WHERE host_key LIKE '%{domain}%' AND name LIKE '%{name}%'";

            var plainBytes = Encoding.ASCII.GetBytes(value);
            var encodedData = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);

            command.Parameters.Add("@sid", SqliteType.Blob, encodedData.Length).Value = encodedData;

            var changedRows = await command.ExecuteNonQueryAsync();

            Console.WriteLine(changedRows);

            connection.Close();

            if (changedRows <= 0)
                throw new Exception("No rows updated.");
        }

        public Task CreateCookie(string domain, string name, string value)
        {
            throw new System.NotImplementedException();
        }

        private static string ChromeAppDataFolder()
        {
            var userPath = Environment.GetEnvironmentVariable(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LOCALAPPDATA" : "Home");


            var path = System.IO.Path.Combine(userPath, "Google/Chrome/User Data/Default");

            return path;
        }
    }
}