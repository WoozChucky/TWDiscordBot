using System;
using System.Threading.Tasks;
using TWDiscordBot.Cookies.Adapters;

namespace TWDiscordBot.Services.TribalWars.Cookies
{
    public class CookiesManager : ICookiesManager
    {
        private ICookieAdapter _adapter;
        
        public CookiesManager()
        {
            _adapter = null;
        }
        
        public async Task CreateOrUpdateCookie(Cookie cookie, BrowserType browser)
        {
            switch (browser)
            {
                case BrowserType.None:
                    _adapter = null;
                    break;
                case BrowserType.GoogleChrome:
                    _adapter = new GoogleChromeAdapter();
                    break;
                default:
                    _adapter = null;
                    break;
            }
            
            await CreateOrUpdate(cookie);
        }

        public async Task<Cookie> GetCookie(string host, string name, BrowserType browser)
        {
            switch (browser)
            {
                case BrowserType.None:
                    _adapter = null;
                    break;
                case BrowserType.GoogleChrome:
                    _adapter = new GoogleChromeAdapter();
                    break;
                default:
                    _adapter = null;
                    break;
            }
            
            if (_adapter == null) 
                throw new NullReferenceException(nameof(_adapter));

            var cookieValue = await _adapter.GetCookie(host, name);

            if (string.IsNullOrEmpty(cookieValue))
            {
                return null;
            }
            
            return new Cookie
            {
                Domain = host,
                Name = name,
                Value = cookieValue
            };
        }

        private async Task CreateOrUpdate(Cookie cookie)
        {
            var localStorageCookie = await _adapter.GetCookie(cookie.Domain, cookie.Name);

            if (string.IsNullOrEmpty(localStorageCookie))
            {
                await _adapter.CreateCookie(cookie.Domain, cookie.Name, cookie.Value);
            }
            else
            {
                await _adapter.UpdateCookie(cookie.Domain, cookie.Name, cookie.Value);
            }
        }

        
    }
}