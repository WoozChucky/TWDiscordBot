using System.Threading.Tasks;

namespace TWDiscordBot.Cookies.Adapters
{
    public interface ICookieAdapter
    {
        Task<string> GetCookie(string domain, string name);
        Task UpdateCookie(string domain, string name, string value);
        Task CreateCookie(string domain, string name, string value);
    }
}