using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using TWDiscordBot.Services.TribalWars.Contracts;
using TWDiscordBot.Services.TribalWars.Model;
using Formatting = Newtonsoft.Json.Formatting;

namespace TWDiscordBot.Services.TribalWars
{
    public class WorldService : IWorldService
    {
        public async Task<WorldConfiguration> GetWorldConfiguration(string world)
        {
            string json;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient
                    .GetStringAsync($"https://{world}.tribalwars.com.pt/interface.php?func=get_config");

                var doc = new XmlDocument();
                doc.LoadXml(response);

                json = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);
            }

            var obj = JsonConvert.DeserializeObject<Response<WorldConfiguration>>(json);
            
            return obj.Config;
        }

        class Response<T>
        {
            public T Config { get; set; }
        }
    }
}