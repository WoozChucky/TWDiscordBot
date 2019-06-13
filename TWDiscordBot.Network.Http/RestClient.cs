using System;
using System.Net.Http;
using System.Threading.Tasks;
using TWDiscordBot.Network.Http.Contracts;

namespace TWDiscordBot.Network.Http
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _client;
        
        protected RestClient(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
        }

        public async Task<string> GetAsStringAsync(string url)
        {
            return await _client.GetStringAsync(url);
        }

        #region Dispose Pattern

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        
    }
}