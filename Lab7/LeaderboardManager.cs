using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace Lab7
{
    // AI helped me with this class, especially the LINQ and JSON part
    // It manages the leaderboard and saves the scores between sessions
    // I told the AI I wanted the leaderboard to be saved locally on the user's computer and I think that's where JSON comes in
    internal class LeaderboardManager
    {
        // Stores all leaderboard entries in memory
        private readonly List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

        // Path to the JSON file where the scores are saved
        private readonly string filePath = Path.Combine(AppContext.BaseDirectory, "Leaderboard.json");

        public List<LeaderboardEntry> GetTopScores(Difficulty difficulty)
        {
            return entries
                .Where(entry => entry.Difficulty == difficulty) // Only entries for this difficulty
                .OrderByDescending(entry => entry.Score) // Highest score first
                .ThenBy(entry => entry.Date) // Earlier entry wins a tie. This means if a new score is the same as the #10 it will not qualify
                .Take(10) // Max 10 entries
                .ToList();
        }

        // Checks whether a new score qualifies for the top ten
        public bool QualifiesForTopTen(int score, Difficulty difficulty)
        {
            List<LeaderboardEntry> topScores = GetTopScores(difficulty);

            // If there are fewer than 10 entries it automatically qualifies
            if (topScores.Count < 10)
            {
                return true;
            }

            // If not then the new score must beat the lowest score in the top ten
            return score > topScores[topScores.Count - 1].Score;
        }

        public void AddScore(string playerName, int score, Difficulty difficulty)
        {
            // If the new score doesn't qualify, the leaderboard ignores it
            if (!QualifiesForTopTen(score, difficulty))
            {
                return;
            }

            // If the new score does qualify it gets added to the leaderboard
            LeaderboardEntry newEntry = new LeaderboardEntry()
            {
                PlayerName = playerName,
                Score = score,
                Difficulty = difficulty,
                Date = DateTime.Now
            };

            entries.Add(newEntry);

            // Groups entries by difficulty and keeps the best ten in each group
            List<LeaderboardEntry> topEntries = entries
                .GroupBy(entry => entry.Difficulty)
                .SelectMany(group => group
                    .OrderByDescending(entry => entry.Score)
                    .ThenBy(entry => entry.Date)
                    .Take(10))
                .ToList();

            // Replaces the old entries with the updated top scores
            entries.Clear();
            entries.AddRange(topEntries);

            SaveScores();
        }

        // Converts the leaderboard entries into JSON and saves them to a file
        public void SaveScores()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true // AI told me this was to make the JSON file easier to read
            };

            string json = JsonSerializer.Serialize(entries, options);

            File.WriteAllText(filePath, json);
        }

        // Loads the saved scores from the JSON file
        public void LoadScores()
        {
            // If there's no leaderboard file, nothing will be loaded
            if (!File.Exists(filePath))
            {
                return;
            }

            string json = File.ReadAllText(filePath);

            // Converts the JSON text back into a list of leaderboard entries
            List<LeaderboardEntry> loadedEntries = JsonSerializer.Deserialize<List<LeaderboardEntry>>(json) ?? new List<LeaderboardEntry>();

            entries.Clear();
            entries.AddRange(loadedEntries);
        }

        // Automatically loads saved scores when a LeaderboardManager is created
        public LeaderboardManager()
        {
            LoadScores();
        }
    }
}
