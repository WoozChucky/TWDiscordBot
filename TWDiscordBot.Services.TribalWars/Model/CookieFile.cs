using System.Collections.Generic;

namespace TWDiscordBot.Services.TribalWars.Model
{
    public class CookieFile
    {
        public ArrayOfCookie ArrayOfCookie { get; set; }
    }

    public class ArrayOfCookie
    {
        public List<Cookie> Cookie { get; set; }
    }

    public class Cookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
    }
}