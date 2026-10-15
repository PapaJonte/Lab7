using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    internal class GameUI
    {
        // This entire menu is completely done by AI since I haven't worked with ASCII before
        // I asked it for an arcade feel for my hangman game
        // I also asked for the options to be highlighted and be chosen by the arrow keys
        public static void DisplayMenu(int selectedIndex)
        {
            Console.Clear();
            Console.WriteLine(
            """
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
            ║                                                                          ║
            ╠══════════════════════════════════════════════════════════════════════════╣
            """
           );

            string[] options =
            {
                "START GAME",
                "DIFFICULTY",
                "STATISTICS",
                "EXIT"
            };

            for (int i = 0; i < options.Length; i++)
            {
                string option = i == selectedIndex
                    ? $"> {options[i]}"
                    : $"  {options[i]}  ";

                Console.Write("║");
                Console.Write(new string(' ', 24));

                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.Write(option);
                Console.ResetColor();

                Console.WriteLine(
                    new string(' ', 74 - 24 - option.Length) + "║"
                );
            }

            Console.WriteLine(
                "╚══════════════════════════════════════════════════════════════════════════╝"
            );
        }
    }
}
