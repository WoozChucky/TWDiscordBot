using CsvHelper.Configuration.Attributes;

namespace TWDiscordBot.Network.Http.TribalWars.Model.World
{
    public class WorldPlayer
    {
        [Index(0)]
        public long Id { get; set; }
        
        [Index(1)]
        public string Name { get; set; }
        
        [Index(2)]
        public long TribeId { get; set; }
        
        [Index(3)]
        public int Villages { get; set; }
        
        [Index(4)]
        public long Points { get; set; }
        
        [Index(5)]
        public int Rank { get; set; }
    }
}