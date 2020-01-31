using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CsvHelper;
using Newtonsoft.Json;
using TWDiscordBot.Network.Http.TribalWars.Contracts;
using TWDiscordBot.Network.Http.TribalWars.Model;
using TWDiscordBot.Network.Http.TribalWars.Model.Configuration;
using TWDiscordBot.Network.Http.TribalWars.Model.World;
using Formatting = Newtonsoft.Json.Formatting;

namespace TWDiscordBot.Network.Http.TribalWars
{
    public class TribalWarsClient : RestClient, ITribalWarsClient
    {
        private const string BaseUrl = "https://{0}.tribalwars.com.pt/";
        
        public TribalWarsClient(IHttpClientFactory factory) : base(factory)
        { }

        public async Task<WorldConfiguration> GetWorldConfiguration(string world)
        {
            var contentResponse = await GetAsStringAsync(BuildBaseUrl(world) + "interface.php?func=get_config");

            var xml = new XmlDocument();
            xml.LoadXml(contentResponse);

            var json = JsonConvert.SerializeXmlNode(xml.LastChild, Formatting.Indented);
            
            return JsonConvert.DeserializeObject<WorldConfigurationResponse>(json).Config;
        }

        public async Task<IEnumerable<WorldPlayer>> GetWorldPlayers(string world)
        {
            var contentResponse = await GetAsStringAsync(BuildBaseUrl(world) + "map/player.txt");

            using var reader = new StringReader(contentResponse);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;

            var players = csv.GetRecords<WorldPlayer>();
            
            return players.ToList();
        }

        private static string BuildBaseUrl(string world)
        {
            return string.Format(BaseUrl, world);
        }
    }
}