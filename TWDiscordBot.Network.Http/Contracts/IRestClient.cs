using System;
using System.Threading.Tasks;

namespace TWDiscordBot.Network.Http.Contracts
{
    public interface IRestClient : IDisposable
    {
        Task<string> GetAsStringAsync(string url);
    }
}