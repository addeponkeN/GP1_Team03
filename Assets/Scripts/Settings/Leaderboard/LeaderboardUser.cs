using Newtonsoft.Json;

namespace Settings.Leaderboard
{
    public class LeaderboardUser
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("score")] public int Score;

        public LeaderboardUser()
        {
        }

        public LeaderboardUser(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}