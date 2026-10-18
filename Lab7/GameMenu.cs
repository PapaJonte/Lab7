using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    internal class GameMenu
    {
        // This is the main menu
        // I originally had the menu choices to be selected with 1-4
        // But I wanted it to feel more Arcade-like, and asked AI if I could make it work with arrow keys
        // I have never worked with ConsoleKey, ReadKey and such so much of this is copied and then explained to me by the AI

        public static void ShowMenu()
        {
            bool isRunning = true;
            int selectedIndex = 0; // The main menu starts with index 0 selected

            while (isRunning)
            {
                GameUI.DisplayMenu(selectedIndex); // Call the DisplayMenu method from GameUI class

                ConsoleKey key = Console.ReadKey(true).Key; // This stores which key the user pressed without requiring Enter

                // This switch statement decides what to do with the key from above
                switch (key)
                {
                    // We use the remainder operator (%) to make sure we always land on the 4 different indexes we have (0, 1, 2 and 3)
                    case ConsoleKey.UpArrow:
                        // For example 1 press on the up arrow key would be (0 - 1 + 4) = 3 no 4's can be removed from remainder and the index becomes 3
                        // The next press would then be (3 - 1 + 4) = 6, we remove one 4 using remainder and we're left with index 2 and so on
                        selectedIndex = (selectedIndex - 1 + 4) % 4; // Furthermore the + 4 is to ensure we're not left with a negative number
                        break;

                    case ConsoleKey.DownArrow:
                        // Similar to the up arrow, down arrow got its own logic
                        // 1 press is 0 + 1 = 1, no 4's can be removed from remainder so we get index 1
                        // Next press is then 1 + 1 = 2, no 4's can be removed from remainder here as well and we get index 2
                        // Two more presses and we're at 3 + 1 = 4, we remove 4 using remainder and we get index 0, where we started
                        selectedIndex = (selectedIndex + 1) % 4;
                        break;

                    case ConsoleKey.Enter:
                        switch (selectedIndex) // Depending on the index we're currently on the Enter key will decide what case is applied
                        {
                            // The 3 different indexes (0-2) run their separate method from the Hangman class
                            case 0: // START GAME
                                Hangman.StartGame();
                                break;

                            case 1: // DIFFICULTY
                                Hangman.SelectDifficulty();
                                break;

                            case 2: // STATISTICS
                                Hangman.ShowStatistics();
                                break;

                            // The 4th index (3), changes isRunning to false, then breaks the inner switch, after this it breaks out of the outer switch
                            // The isRunning condition is checked again, but since it's now false the while-loop ends
                            // Since Program.cs has nothing else in it other than ShowMenu() the program then ends
                            case 3: // EXIT
                                isRunning = false;
                                break;
                        }
                        break;
                }
            }
        }

        // This is the Game Over Menu, that receives the result of the game, the correct answer and the final score
        // It retuns a bool depending on if the user want to play again or return to the main menu
        public static bool ShowGameOverMenu(bool hasWon, bool hasForfeited, string answer, int score)
        {
            int selectedIndex = 0;
            bool isSelecting = true;
            bool playAgain = false;

            while (isSelecting)
            {
                GameUI.DisplayGameOver(hasWon, hasForfeited, answer, score, selectedIndex);

                ConsoleKey key = Console.ReadKey(true).Key;

                // This is very similar to the previous menu, only that it has 2 index instead of 4, hence the % 2
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + 2) % 2;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % 2;
                        break;

                    case ConsoleKey.Enter:
                        return selectedIndex == 0; // Returns true for Play Again (index 0), and false for Main Menu (index 1)
                }
            }

            return playAgain;
        }
    }
}
