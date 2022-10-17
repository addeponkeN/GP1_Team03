using System.Collections.Generic;
using Newtonsoft.Json;

namespace Settings.Leaderboard
{
    public class LeaderboardFile : RootFile
    {
        [JsonProperty("list")] public List<LeaderboardUser> List;
    }
}