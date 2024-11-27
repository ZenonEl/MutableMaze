using System;

namespace MutableMaze
{
    public class GameMenu
    {
        public static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Game Menu:");
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Load Save");
            Console.WriteLine("3. Help");
            Console.WriteLine("4. History");
            Console.WriteLine("5. Settings");
            Console.WriteLine("6. Exit");
        }
        
        public static void PrintHistory()
        {
            Console.WriteLine("Enter the name of the save file:");
        }

        public static void PrintSettings()
        {
            Console.WriteLine("Your current settings:");
        }

        public static void PrintHelp()
        {
            Console.Clear();
            Console.WriteLine("Help Menu:");
            Console.WriteLine("1. How to play");
            Console.WriteLine("2. Controls");
            Console.WriteLine("3. Back to game menu");
            Console.WriteLine("Press enter to continue");
            string _ = Console.ReadLine();
            PrintMenu();
            GetChoiceInput();
        }

        public static void GetChoiceInput()
        {
            Console.WriteLine("Enter your choice:");
            string input = Console.ReadLine();

            input = input.Trim();

            switch (input)
            {
                case "1":
                    GameLogic.StartGame(); // v1 done
                    PrintMenu();
                    GetChoiceInput();
                    break;
                case "2":
                    GameLogic.LoadGame();
                    break;
                case "3":
                    PrintHelp(); // v1 done
                    break;
                case "4":
                    PrintHistory();
                    break;
                case "5":
                    PrintSettings();
                    break;
                case "6": // done
                    Console.Clear();
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
