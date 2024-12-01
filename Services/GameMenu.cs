using System;

namespace MutableMaze
{
    public class GameMenu
    {
        public static List<string> allHistoryFiles = []; 
        private static GameConfig config = GameConfig.Instance;
        private static string helpText = @"To start playing you just need to select option 1 in the menu. 
    Control in the labyrinth is carried out via arrows on the keyboard or via WASD. After reaching the exit, the game stops. You will be presented with brief statistics for the game and then you can return to the main menu.

There is also an option to save the game and load it in the future. This is done by pressing Ctr + S. 
    After saving the game you can continue playing or exit. 

Also be careful when pressing any other keys. Because all other keys lead you to exit the game session. 
    But before exiting you will be reminded to first make a save or just continue the game by pressing the control keys

Now a little bit about the features of the game. As the name suggests, the maze can and will mutate from the settings. 
    In other words, it will be rebuilt/regenerated right in the process of your game. 
    And to combat what I considered to be some of the abuses of this mechanic, I've made some defenses. 
    The maze regenerates after the player has traveled a certain number of cells. But in order for this count to start, the game first checks how far the player has traveled from the last point he was at when the maze was last regenerated. 
    If the player is within the radius (which is set in % in the config) of this point, the maze regeneration trigger will not be filled. As soon as the player passes beyond this radius the regeneration trigger will start to accumulate";

        public static void PrintMenu()
        {
            Console.Clear();
            Utils.WriteLine("Game Menu:", ConsoleColor.Yellow);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Load Save");
            Console.WriteLine("3. Help");
            Console.WriteLine("4. History");
            Console.WriteLine("5. Settings");
            Utils.WriteLine("6. Exit", ConsoleColor.Red);
        }
        
        public static void PrintHistory()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("GameSaves/History");

            FileInfo[] files = directoryInfo.GetFiles();
            int index = 0;
            Utils.WriteLine("History:", ConsoleColor.Yellow);
            foreach (FileInfo file in files)
            {
                Utils.WriteLine($"{index}. {file.Name}", ConsoleColor.Green);
                allHistoryFiles.Add(file.Name);
                index++;
            }
            Utils.WriteLine("Press enter to continue", ConsoleColor.DarkCyan);
            Console.ReadLine();
        }

        public static void PrintSettings()
        {
            Utils.WriteLine("Your current settings:", ConsoleColor.DarkCyan);
            config.LoadConfig("Config/config.json");
            Console.WriteLine($@"1) Width and Height: {config.Maze.Width} {config.Maze.Height} 
2) Start and Exit Symbols: {config.Maze.Symbols.Start} {config.Maze.Symbols.Exit} 
3) Wall and Path Symbols: {config.Maze.Symbols.Wall} {config.Maze.Symbols.Path} 
4) Player Symbol: {config.Player.Symbol} 
5) Regeneration Trigger: {config.Maze.RegenerationTrigger.Value}
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
                    Console.WriteLine("Enter the new move regeneration value:");
                    choice = Console.ReadLine();
                    config.Maze.RegenerationTrigger.Value = int.Parse(choice.Split(" ")[0]);
                    break;

            }
            config.SaveConfig("Config/config.json");
            Utils.WriteLine("Settings saved. Return to menu? Write Yes (Y/y) or press enter to continue editing settings.", ConsoleColor.DarkCyan);
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
            Utils.WriteLine("Help Menu:", ConsoleColor.Yellow);
            Utils.WriteLine(helpText, ConsoleColor.Green);
            Utils.WriteLine("Press enter to continue", ConsoleColor.DarkCyan);
            Console.ReadLine();
            PrintMenu();
            GetChoiceInput();
        }

        public static void GetChoiceInput()
        {
            Utils.WriteLine("Enter your choice:", ConsoleColor.DarkCyan);
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
                case "":
                    Console.Clear();
                    PrintMenu();
                    GetChoiceInput();
                    break;
            }
        }
    }
}
