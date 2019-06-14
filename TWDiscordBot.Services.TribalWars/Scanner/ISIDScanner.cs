using System;
using System.Threading.Tasks;
using TWDiscordBot.Services.TribalWars.Cookies;

namespace TWDiscordBot.Services.TribalWars.Scanner
{
    public interface ISIDScanner
    {
        event EventHandler<Cookie> NewSID;

        Task StartScan();
        Task StopScan();
    }
}