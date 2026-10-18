using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    // This class stores the information for each leaderboard entry
    internal class LeaderboardEntry
    {
        public string PlayerName { get; set; } = "";

        public int Score { get; set; }

        public Difficulty Difficulty { get; set; }

        public DateTime Date { get; set; }
    }
}
