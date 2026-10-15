using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    internal class GameMenu
    {
        public static void ShowMenu()
        {
            bool isRunning = true;
            int selectedIndex = 0;

            while (isRunning)
            {
                // Never had menu choices be with arrow keys
                // So this AI helped me with when I said that I wanted to use arrow keys
                GameUI.DisplayMenu(selectedIndex);

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + 4) % 4;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % 4;
                        break;

                    case ConsoleKey.Enter:
                        switch (selectedIndex)
                        {
                            case 0: // START GAME
                                Hangman.StartGame();
                                break;

                            case 1: // DIFFICULTY
                                Hangman.SelectDifficulty();
                                break;

                            case 2: // STATISTICS
                                Hangman.ShowStatistics();
                                break;

                            case 3: // EXIT
                                isRunning = false;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
