using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using TWDiscordBot.Network.Http.TribalWars.Contracts;
using TWDiscordBot.Network.Http.TribalWars.Model;
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

        private static string BuildBaseUrl(string world)
        {
            return string.Format(BaseUrl, world);
        }
    }
}