using Newtonsoft.Json;

namespace TWDiscordBot.Network.Http.TribalWars.Model.Configuration
{
    public class WorldAllyConfiguration
    {
        [JsonProperty("no_harm")]
        public int NoHarm { get; set; }
        
        [JsonProperty("no_other_support")]
        public int NoOtherSupport { get; set; }
        
        [JsonProperty("allytime_support")]
        public int AllyTimeSupport { get; set; }
        
        [JsonProperty("limit")]
        public int Limit { get; set; }
        
        [JsonProperty("fixed_allies")]
        public int FixedAllies { get; set; }
        
        [JsonProperty("points_member_count")]
        public int PointsMemberCount { get; set; }
        
        [JsonProperty("wars_member_requirement")]
        public int WarsMemberRequirement { get; set; }
        
        [JsonProperty("wars_points_requirement")]
        public int WarsPointsRequirement { get; set; }
        
        [JsonProperty("wars_autoaccept_days")]
        public int WarsAutoAcceptDays { get; set; }
        
        [JsonProperty("levels")]
        public int Levels { get; set; }
        
        [JsonProperty("xp_requirements")]
        public string XpRequirements { get; set; }
    }
}