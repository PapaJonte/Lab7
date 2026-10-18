using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    // For Hell mode I decided that I wanted the user to guess whole sentences instead of words
    // I asked the UI to generate sentences and it gave me the suggestion to divide it in 4 different sections
    internal class SentenceGenerator
    {
        private readonly string[] subjects =
        {
            "THE WIZARD",
            "THE KNIGHT",
            "THE DRAGON",
            "THE PIRATE",
            "THE KING",
            "THE THIEF",
            "THE HUNTER"
        };

        private readonly string[] verbs =
        {
            "FOUND",
            "DESTROYED",
            "DISCOVERED",
            "PROTECTED",
            "STOLE",
            "FOLLOWED",
            "WATCHED"
        };

        private readonly string[] objects =
        {
            "THE ANCIENT SWORD",
            "A HIDDEN TREASURE",
            "THE GOLDEN CROWN",
            "A MYSTERIOUS PORTAL",
            "THE LOST KINGDOM",
            "A SECRET MESSAGE"
        };

        private readonly string[] locations =
        {
            "IN THE FOREST",
            "BEHIND THE CASTLE",
            "UNDER THE MOUNTAIN",
            "NEAR THE RIVER",
            "INSIDE THE DUNGEON",
            "BEYOND THE OCEAN"
        };

        // Keeps track of previously generated sentences to prevent repeating them
        private readonly HashSet<string> usedSentences = new HashSet<string>();

        public string GenerateSentence()
        {
            // Calculates the total number of possible sentence combinations (7 * 7 * 6 * 6 = 1764)
            int totalCombinations = subjects.Length * verbs.Length * objects.Length * locations.Length;

            // If all possible combinations are reached, clear the history
            if (usedSentences.Count >= totalCombinations)
            {
                usedSentences.Clear();
            }

            string sentence;

            // A do-while-loop always exectues at least once, which is perfect for my goal, since I always want a sentence to be generated
            do
            {
                // Randomly selects one entry from each array to complete the sentence
                string subject = subjects[Random.Shared.Next(subjects.Length)];
                string verb = verbs[Random.Shared.Next(verbs.Length)];
                string objectName = objects[Random.Shared.Next(objects.Length)];
                string location = locations[Random.Shared.Next(locations.Length)];

                sentence = $"{subject} {verb} {objectName} {location}.";

            } while (usedSentences.Contains(sentence));

            // Save the sentence to prevent it from being generated again
            usedSentences.Add(sentence);

            return sentence;
        }
    }
}
