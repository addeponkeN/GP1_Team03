using System.Collections.Generic;
using Newtonsoft.Json;

namespace Settings.Leaderboard
{
    public class LeaderboardFile
    {
        [JsonProperty("list")] public List<LeaderboardUser> List;
    }
}