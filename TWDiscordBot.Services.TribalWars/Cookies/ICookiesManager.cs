using System.Threading.Tasks;

namespace TWDiscordBot.Services.TribalWars.Cookies
{
    public interface ICookiesManager
    {
        Task CreateOrUpdateCookie(Cookie cookie, BrowserType browser);
        Task<Cookie> GetCookie(string host, BrowserType type);
    }
}