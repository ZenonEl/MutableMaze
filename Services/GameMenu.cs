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
            Console.WriteLine($@"1) Width and Height: {config.Maze.Width} {config.Maze.Height} 
2) Start and Exit Symbols: {config.Maze.Symbols.Start} {config.Maze.Symbols.Exit} 
3) Wall and Path Symbols: {config.Maze.Symbols.Wall} {config.Maze.Symbols.Path} 
4) Player Symbol: {config.Player.Symbol} 
5) Regeneration Trigger: {config.Maze.RegenerationTrigger.Value} {config.Maze.RegenerationTrigger.Type}
Write the number of the setting you want to change, or press enter to go back to the menu. After writing the number, press enter and write the new value.
Example: 1 *press enter*
51 21");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter the new width and height:");
                    choice = Console.ReadLine();
                    config.Maze.Width = int.Parse(choice.Split(" ")[0]);
                    config.Maze.Height = int.Parse(choice.Split(" ")[1]);
                    break;
                case "2":
                    Console.WriteLine("Enter the new start and exit symbols:");
                    choice = Console.ReadLine();
                    config.Maze.Symbols.Start = choice.Split(" ")[0][0];
                    config.Maze.Symbols.Exit = choice.Split(" ")[1][0];
                    break;
                case "3":
                    Console.WriteLine("Enter the new wall and path symbols:");
                    choice = Console.ReadLine();
                    config.Maze.Symbols.Wall = choice.Split(" ")[0][0];
                    config.Maze.Symbols.Path = choice.Split(" ")[1][0];
                    break;
                case "4":
                    Console.WriteLine("Enter the new player symbol:");
                    choice = Console.ReadLine();
                    config.Player.Symbol = choice[0];
                    break;
                case "5":
                    Console.WriteLine("Enter the new regeneration trigger:");
                    choice = Console.ReadLine();
                    config.Maze.RegenerationTrigger.Value = int.Parse(choice.Split(" ")[0]);
                    config.Maze.RegenerationTrigger.Type = choice.Split(" ")[1];
                    break;

            }
            config.SaveConfig("Config/config.json");
            Console.WriteLine("Settings saved. Return to menu? Write Yes (Y/y) or press enter to continue editing settings.");
            choice = Console.ReadLine();
            List<string> yesAnswers = ["yes", "y", "Yes", "Y"];
            if (yesAnswers.Contains(choice))
            {
                return;
            }
            PrintSettings();
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
                    PrintSettings(); // v1 done
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
