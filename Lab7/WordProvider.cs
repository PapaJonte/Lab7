using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Lab7
{
    // I originally planned on doing a list with just tons of words but ended up asking the AI for a better way to do so
    // AI suggested working with LINQ, so this class is made hugely with the help of AI
    // I have worked with LINQ before as well as pointing to BaseDirectory and such, but not recent enough to remember it
    internal class WordProvider
    {
        // A list that stores all the words loaded from the text file
        private readonly List<string> words;

        public WordProvider()
        {
            // Creates the parth to the Words.txt file inside the Data folder
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "Words.txt");

            // Reads all lines from the file and use LINQ to filter the words
            words = File.ReadAllLines(path)
            // The reason I filter words that are at least 4 characters is due to it being the lower limit of Easy mode
            // I asked the AI to google the 10,000 most common English words and copied that list, a lot of the words were less than 4
            .Where(word => word.Length >= 4)
            .Where(word => word.All(char.IsLetter)) // Safeguard in case there would be any word with something else than a letter
            .Select(word => word.ToUpperInvariant()) // Converts all words to uppercase
            .Distinct() // This removes duplicate words
            .ToList(); // Store the results in a List<string>
        }

        // Keeps track of the words already used to prevent getting the same word twice
        private readonly HashSet<string> usedWords = new HashSet<string>();

        public string GetRandomWord(Difficulty difficulty)
        {
            List<string> availableWords = words
                .Where(word => difficulty switch
                {
                    // This filters the word based on the requirement I set for the different difficulties
                    Difficulty.Easy => word.Length >= 4 && word.Length <= 6,
                    Difficulty.Normal => word.Length >= 7 && word.Length <= 9,
                    Difficulty.Hard => word.Length >= 10,
                    _ => false
                    
                })
                .Where(word => !usedWords.Contains(word)) // Removes words that have already been selected in the previous rounds
                .ToList();

            // If all words for a difficulty have been used this clears the history of used words and selects a new word
            if (availableWords.Count == 0)
            {
                usedWords.Clear();
                return GetRandomWord(difficulty);
            }

            string selectedWord = availableWords[Random.Shared.Next(availableWords.Count)]; // Randomly selects a word from the available list

            usedWords.Add(selectedWord); // Store the word so it cannot be selected again

            return selectedWord;
        }
    }
}
