using System.Threading.Tasks;

namespace TWDiscordBot.Cookies.Adapters
{
    public class MozillaFirefoxAdapter : ICookieAdapter
    {
        public Task<string> GetCookie(string domain, string name)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateCookie(string domain, string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateCookie(string domain, string name, string value)
        {
            throw new System.NotImplementedException();
        }
    }
}