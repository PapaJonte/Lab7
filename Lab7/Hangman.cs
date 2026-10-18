using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab7
{
    internal class Hangman
    {
        // These objects handle word selection, sentence generation and the leaderboard
        // They are created once and reused throughout the game
        private static readonly WordProvider wordProvider = new WordProvider();
        private static readonly SentenceGenerator sentenceGenerator = new SentenceGenerator();
        private static readonly LeaderboardManager leaderboardManager = new LeaderboardManager();

        // Stores the currently selected difficulty, starting with Easy by default
        public static Difficulty CurrentDifficulty { get; set; } = Difficulty.Easy;

        public static void StartGame()
        {
            bool playAgain = true;

            while (playAgain)
            {
                // Picks a word or sentence (if Hell mode)
                string answer = CurrentDifficulty switch
                {
                    Difficulty.Easy => wordProvider.GetRandomWord(Difficulty.Easy),
                    Difficulty.Normal => wordProvider.GetRandomWord(Difficulty.Normal),
                    Difficulty.Hard => wordProvider.GetRandomWord(Difficulty.Hard),
                    Difficulty.Hell => sentenceGenerator.GenerateSentence(),
                    _ => throw new InvalidOperationException("Invalid difficulty.") // AI suggested a fallback case for unexpected values
                };

                // Creates new collections for each round to track guessed letters and incorrect guesses
                HashSet<char> guessedLetters = new HashSet<char>();
                HashSet<char> incorrectGuesses = new HashSet<char>();

                // Every round starts with 6 attempts and resets the game outcome
                int remainingAttempts = 6;
                bool isPlaying = true;
                bool hasWon = false;
                bool hasForfeited = false;

                while (isPlaying)
                {
                    // Keep the game in the normal color, even if Hell mode is selected
                    Console.ResetColor();
                    Console.Clear();

                    Console.WriteLine($"Difficulty: {CurrentDifficulty}");
                    Console.WriteLine($"Remaining attempts: {remainingAttempts}/6");
                    Console.WriteLine();

                    // Update the gallow image based on remaining attempts
                    GameUI.DisplayHangman(remainingAttempts);

                    // Displays the letters the use has already guessed
                    Console.WriteLine($"Guessed letters: {string.Join(", ", guessedLetters)}");
                    Console.WriteLine($"Incorrect guesses: {string.Join(", ", incorrectGuesses)}");
                    Console.WriteLine();

                    // AI helped me with this one to get the _ _ _ _ instead of letters in the beginning that's later changed to the correct letters
                    string hiddenAnswer = string.Join(" ", answer.Select(character =>
                        char.IsLetter(character) && !guessedLetters.Contains(character)
                            ? "_"
                            : character.ToString()
                    ));

                    Console.WriteLine(hiddenAnswer);
                    Console.WriteLine();

                    // Checks if every letter in the answer has been guessed
                    hasWon = answer
                        .Where(char.IsLetter)
                        .All(character => guessedLetters.Contains(character));

                    // Ends the round if the user wins or runs out of attempts
                    if (hasWon || remainingAttempts == 0)
                    {
                        isPlaying = false;
                        continue;
                    }

                    Console.Write("Guess a letter (Esc to forfeit): ");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    // Pressing Esc forfeits the round without counting as a wrong guess
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        hasForfeited = true;
                        isPlaying = false;
                        continue;
                    }

                    // Converts the input to be uppercase to match the uppercase answer
                    char guess = char.ToUpperInvariant(keyInfo.KeyChar);

                    // Ignores inputs that aren't letters
                    if (!char.IsLetter(guess))
                    {
                        continue;
                    }

                    // Ignores letters that has already been guessed
                    if (guessedLetters.Contains(guess) || incorrectGuesses.Contains(guess))
                    {
                        continue;
                    }

                    // Correct guesses are saved while incorrect guesses reduce the remaining attempts
                    if (answer.Contains(guess))
                    {
                        guessedLetters.Add(guess);
                    }
                    else
                    {
                        incorrectGuesses.Add(guess);
                        remainingAttempts--;
                    }
                }

                // Calculates and save the score if the user has won
                if (hasWon && !hasForfeited)
                {
                    int score = CalculateScore(answer, remainingAttempts);

                    // Checks the leaderboard to see if the score is high enough to make it to the top ten
                    if (leaderboardManager.QualifiesForTopTen(score, CurrentDifficulty))
                    {
                        Console.WriteLine($"Your score: {score}");
                        Console.WriteLine("NEW TOP 10 SCORE!");
                        Console.WriteLine();

                        // If it is high enough it asks the user to write down their name
                        Console.WriteLine("Enter your name: ");
                        string playerName = Console.ReadLine() ?? string.Empty;

                        // If the user doesn't write a name it saves it as Anonymous
                        if (string.IsNullOrWhiteSpace(playerName))
                        {
                            playerName = "Anonymous";
                        }

                        // Adds it to the leaderboard
                        leaderboardManager.AddScore(playerName, score, CurrentDifficulty);
                    }
                }

                // Calculates the final score for a win and return it, regardless if the user makes the top ten or not
                int finalScore = hasWon && !hasForfeited ? CalculateScore(answer, remainingAttempts) : 0;
                playAgain = GameMenu.ShowGameOverMenu(hasWon, hasForfeited, answer, finalScore);
            }
        }

        // Calculator for the final score based on the answer length and remaining attempts
        private static int CalculateScore(string answer, int remainingAttempts)
        {
            // Only counts letters, this is mainly used to avoid it counting spaces and . in Hell mode
            int letterCount = answer.Count(char.IsLetter);

            // 10 points per letter and 25 points per remaining attempt left
            int score = (letterCount * 10) + (remainingAttempts * 25);

            return score;
        }

        public static void SelectDifficulty()
        {
            // Contains the 4 available difficulty levels
            Difficulty[] difficulties =
            {
                Difficulty.Easy,
                Difficulty.Normal,
                Difficulty.Hard,
                Difficulty.Hell
            };

            // Starts the selection on the currently active difficulty
            int selectedIndex = (int)CurrentDifficulty; // (int) converts the enum value from Difficulty.cs to its numerical value
            bool isSelecting = true;

            while (isSelecting)
            {
                // This is again to differentiate Hell from the other difficulties color-wise
                if (difficulties[selectedIndex] == Difficulty.Hell)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Clear();
                Console.WriteLine("=== SELECT DIFFICULTY ===");
                Console.WriteLine();

                // Highlights the current selected index
                for (int i = 0; i < difficulties.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = difficulties[i] == Difficulty.Hell ? ConsoleColor.Red : ConsoleColor.Yellow;

                        Console.WriteLine($"> {difficulties[i]}");

                        Console.ForegroundColor = difficulties[selectedIndex] == Difficulty.Hell ? ConsoleColor.Black : ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine($"  {difficulties[i]}");
                    }
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                // Up and down arrow navigates in the menu, Enter saves the selected difficulty for the game and Esc exit the menu without choosing difficulty
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + difficulties.Length) % difficulties.Length;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % difficulties.Length;
                        break;

                    case ConsoleKey.Enter:
                        CurrentDifficulty = difficulties[selectedIndex];
                        isSelecting = false;
                        break;

                    case ConsoleKey.Escape:
                        isSelecting = false;
                        break;
                }
            }

            Console.ResetColor();
            Console.Clear();
        }

        public static void ShowStatistics()
        {
            // This resets the color to default, simply because I don't want Hell color scheme to transfer over in here
            Console.ResetColor();
            Console.Clear();

            Difficulty[] difficulties =
            {
                Difficulty.Easy,
                Difficulty.Normal,
                Difficulty.Hard,
                Difficulty.Hell
            };

            int selectedIndex = 0;
            bool isViewing = true;

            while (isViewing)
            {
                Console.Clear();

                Console.WriteLine("=== LEADERBOARDS ===");
                Console.WriteLine();

                // Display difficulty navigation
                Console.WriteLine($"< {difficulties[selectedIndex].ToString().ToUpper()} >");
                Console.WriteLine();

                // Gets the top ten scores for the difficulty currently being viewed
                List<LeaderboardEntry> topScores = leaderboardManager.GetTopScores(difficulties[selectedIndex]);

                // If there's no scores to be displayed, the game tell the user it here
                if (topScores.Count == 0)
                {
                    Console.WriteLine("No scores yet.");
                }
                else
                {
                    // AI helped me with the visuals here
                    Console.WriteLine("RANK  PLAYER               SCORE  DATE");
                    Console.WriteLine("---------------------------------------------");

                    // Displays the top entries
                    for (int i = 0; i < topScores.Count; i++)
                    {
                        LeaderboardEntry entry = topScores[i];

                        // Highlight the top three positions
                        if (i == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else if (i == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else if (i == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        else
                        {
                            Console.ResetColor();
                        }

                        Console.WriteLine($"{i + 1,2}.   {entry.PlayerName,-20} {entry.Score,5}  {entry.Date:yyyy-MM-dd}");
                    }

                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.WriteLine("Left/Right: Change difficulty");
                Console.WriteLine("Escape: Return to main menu");

                ConsoleKey key = Console.ReadKey(true).Key;

                // Similar to previous, but this makes it so the user can navigate the different leaderboards with left and right arrow key
                // With Esc key exiting out of the leaderboards
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        selectedIndex =
                            (selectedIndex - 1 + difficulties.Length) % difficulties.Length;
                        break;

                    case ConsoleKey.RightArrow:
                        selectedIndex =
                            (selectedIndex + 1) % difficulties.Length;
                        break;

                    case ConsoleKey.Escape:
                        isViewing = false;
                        break;
                }
            }

            Console.ResetColor();
            Console.Clear();
        }
    }
}