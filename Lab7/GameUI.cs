using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    internal class GameUI
    {
        // This entire menu is completely done by AI since I haven't worked with ASCII before
        // It's entirely copied with some adjustements to color schemes
        public static void DisplayMenu(int selectedIndex)
        {
            // This one I added since I wanted the main menu to be changed if Hell mode was selected
            bool isHellMode = Hangman.CurrentDifficulty == Difficulty.Hell;

            if (isHellMode)
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
            string currentDifficulty = Hangman.CurrentDifficulty.ToString();
            // I asked for an arcade-feel to my main menu
            // I asked how to add so that the difficulty is displayed in the bottom right corner
            Console.WriteLine(
            $"""
            ╔══════════════════════════════════════════════════════════════════════════╗
            ║                                                                          ║
            ║  ██╗  ██╗ █████╗ ███╗   ██╗ ██████╗ ███╗   ███╗ █████╗ ███╗   ██╗        ║
            ║  ██║  ██║██╔══██╗████╗  ██║██╔════╝ ████╗ ████║██╔══██╗████╗  ██║        ║
            ║  ███████║███████║██╔██╗ ██║██║  ███╗██╔████╔██║███████║██╔██╗ ██║        ║
            ║  ██╔══██║██╔══██║██║╚██╗██║██║   ██║██║╚██╔╝██║██╔══██║██║╚██╗██║        ║
            ║  ██║  ██║██║  ██║██║ ╚████║╚██████╔╝██║ ╚═╝ ██║██║  ██║██║ ╚████║        ║
            ║  ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝        ║
            ║                                                                          ║
            ║                      +--------+                                          ║
            ║                      |        O                                          ║
            ║                      |       /|\                                         ║
            ║                      |       / \                                         ║
            ║                      |                                                   ║
            ║                  ____|____                                               ║
            ║                                                          {currentDifficulty,-16}║
            ╠══════════════════════════════════════════════════════════════════════════╣
            """
           );

            // The options below the ASCII image
            string[] options =
            {
                "START GAME",
                "DIFFICULTY",
                "STATISTICS",
                "EXIT"
            };

            // Make it so it shows a little arrow ">" as well as highlight the option you currently are on
            // And again, make sure that the design differs depending on if you've selected Hell mode or not
            for (int i = 0; i < options.Length; i++)
            {
                string option = i == selectedIndex ? $"> {options[i]}" : $"  {options[i]}  ";

                Console.Write("║");
                Console.Write(new string(' ', 24));

                if (i == selectedIndex)
                {
                    Console.ForegroundColor = isHellMode ? ConsoleColor.Red : ConsoleColor.Yellow;
                }

                Console.Write(option);

                Console.ForegroundColor = isHellMode ? ConsoleColor.Black : ConsoleColor.White;

                Console.WriteLine(new string(' ', 74 - 24 - option.Length) + "║");
            }

            Console.WriteLine(
                "╚══════════════════════════════════════════════════════════════════════════╝"
            );
        }

        // This displays the 3 different versions you get when you finish a game
        public static void DisplayGameOver(bool hasWon, bool hasForfeited, string answer, int score, int selectedIndex)
        {
            Console.Clear();

            if (hasWon)
            {
                Console.ForegroundColor = ConsoleColor.Green; // Green for success
                Console.WriteLine("=== YOU WIN! ===");
            }
            else if (hasForfeited)
            {
                Console.ForegroundColor = ConsoleColor.Yellow; // Yellow for forfeiting
                Console.WriteLine("=== FORFEITED! ===");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; // Red for Game Over
                Console.WriteLine("=== GAME OVER! ===");
                Console.WriteLine();

                DisplayHangman(0); // Also added the complete hanged man as a visual for dramatic effect. 0 = 0 remaining attempts as shown below
            }

            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine($"Answer: {answer}"); // Prints out the correct answer

            // If the user managed to guess correctly, it will display the final score
            // I had to add this, since there was a chance where the max score was shown if you forfeited
            if (hasWon)
            {
                Console.WriteLine($"Final score: {score}");
            }

            Console.WriteLine();

            string[] options = { "Play Again", "Main Menu" }; // Gives the user the option to play again or return to main menu

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }
        }

        // This is the gallow design, step by step. I chose to make it simple and let the user have 6 incorrect guesses, regardless of difficulty
        public static void DisplayHangman(int remainingAttempts)
        {
            string[] stages =
            {
                // 0 incorrect guesses
                """
                        +--------+
                        |
                        |
                        |
                        |
                    ____|____
                """,

                // 1 incorrect guess - Head
                """
                        +--------+
                        |        O
                        |
                        |
                        |
                    ____|____
                """,

                // 2 incorrect guesses - Body
                """
                        +--------+
                        |        O
                        |        |
                        |
                        |
                    ____|____
                """,

                // 3 incorrect guesses - Left arm
                """
                        +--------+
                        |        O
                        |       /|
                        |
                        |
                    ____|____
                """,

                // 4 incorrect guesses - Right arm
                """
                        +--------+
                        |        O
                        |       /|\
                        |
                        |
                    ____|____
                """,

                // 5 incorrect guesses - Left leg
                """
                        +--------+
                        |        O
                        |       /|\
                        |       /
                        |
                    ____|____
                """,

                // 6 incorrect guesses - Complete
                """
                        +--------+
                        |        O
                        |       /|\
                        |       / \
                        |
                    ____|____
                """
            };

            // Math.Clamp ensures that the value stays between min and max: Math.Clamp(value, min, max)
            // In our case it becomes: value = (6 - remainingAttempts), min = 0, max = 6
            int incorrectGuesses = Math.Clamp(6 - remainingAttempts, 0, 6);

            Console.WriteLine(stages[incorrectGuesses]);
            Console.WriteLine();
        }
    }
}
