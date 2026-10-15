using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab7
{
    internal class Hangman
    {
        public static void StartGame()
        {

        }

        public static Difficulty CurrentDifficulty { get; set; } = Difficulty.Easy;

        public static void SelectDifficulty()
        {
            Difficulty[] difficulties =
            {
                Difficulty.Easy,
                Difficulty.Normal,
                Difficulty.Hard,
                Difficulty.Hell
            };

            int selectedIndex = (int)CurrentDifficulty;
            bool isSelecting = true;

            while (isSelecting)
            {
                Console.Clear();
                Console.WriteLine("=== SELECT DIFFICULTY ===");
                Console.WriteLine();

                for (int i = 0; i < difficulties.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        if (difficulties[i] == Difficulty.Hell)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        Console.WriteLine($"> {difficulties[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {difficulties[i]}");
                    }
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex =
                            (selectedIndex - 1 + difficulties.Length)
                            % difficulties.Length;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex =
                            (selectedIndex + 1)
                            % difficulties.Length;
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
        }

        public static void ShowStatistics()
        {

        }
    }
}
