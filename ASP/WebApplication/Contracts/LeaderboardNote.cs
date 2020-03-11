using Newtonsoft.Json;
using System;

namespace WebApplication.Contracts
{
    [JsonObject, Serializable]
    public class LeaderboardNote
    {
        public float timeInGame { get; set; }
        public int score { get; set; }
    }
}
