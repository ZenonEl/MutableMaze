using System;

namespace MutableMaze
{
    public class GameMenu
    {
        public static List<string> allHistoryFiles = []; 
        private static GameConfig config = GameConfig.Instance;

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
            DirectoryInfo directoryInfo = new DirectoryInfo("GameSaves/History");

            FileInfo[] files = directoryInfo.GetFiles();
            int index = 0;
            Console.WriteLine("History:");
            foreach (FileInfo file in files)
            {
                Console.WriteLine($"{index}. {file.Name}");
                allHistoryFiles.Add(file.Name);
                index++;
            }
            Console.ReadLine();
        }

        public static void PrintSettings()
        {
            Console.WriteLine("Your current settings:");
            Console.WriteLine($@"{config.Maze.Width} {config.Maze.Height} 
{config.Maze.Symbols.Start} {config.Maze.Symbols.Exit} 
{config.Maze.Symbols.Wall} {config.Maze.Symbols.Path} 
{config.Player.Symbol} 
{config.Maze.RegenerationTrigger.Type} {config.Maze.RegenerationTrigger.Value}");
            Console.ReadLine();
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Help Menu:");
            Console.WriteLine("1. How to play");
            Console.WriteLine("2. Controls");
            Console.WriteLine("3. Back to game menu");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
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
                    Console.Clear();
                    GameLogic.StartGame(); // v1 done
                    PrintMenu();
                    GetChoiceInput();
                    break;
                case "2":
                    Console.Clear();
                    GameLogic.LoadGame(); // v1 done
                    PrintMenu();
                    GetChoiceInput();
                    break;
                case "3":
                    Console.Clear();
                    PrintHelp(); // v1 done
                    PrintMenu();
                    GetChoiceInput();
                    break;
                case "4":
                    Console.Clear();
                    PrintHistory(); // v1 done
                    PrintMenu();
                    GetChoiceInput();
                    break;
                case "5":
                    Console.Clear();
                    PrintSettings();
                    PrintMenu();
                    GetChoiceInput();
                    break;
                case "6": // done
                    Console.Clear();
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
