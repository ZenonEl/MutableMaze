namespace MutableMaze
{
    public class GameLogic
    {
        private static GameConfig config = GameConfig.Instance;
        private static ReGeneratedMaze maze;
        private static int movesToRegenMaze = 0;
        private static int allmoves = 0;
        private static (int x, int y) playerLastRegenerationPosition = (0, 0);

        public static void StartGame()
        {
            maze = new(config.Maze.Width, config.Maze.Height, (1, 1), (config.Maze.Width - 2, config.Maze.Height - 2), 
                        (config.Player.StartX, config.Player.StartY), config.Maze.Symbols.Start, config.Maze.Symbols.Exit, config.Maze.Symbols.Wall, 
                        config.Maze.Symbols.Path, config.Player.Symbol);
            
            maze.PrintMaze();
            ProcessGameInputTriggers();
        }

        public static void LoadGame()
        {

            int index = 0;
            int choice;
            string filename;
            string[] jsonFiles = Directory.GetFiles("GameSaves", "*.json");
            List<string> allSavesFiles = []; 
            
            Utils.WriteLine("Your saves:", ConsoleColor.Yellow);
            
            foreach (string file in jsonFiles)
            {
                filename = Path.GetFileName(file);
                Utils.WriteLine($"{index}. {filename}", ConsoleColor.Green); 
                allSavesFiles.Add(filename);
                index++;
            }
            Utils.WriteLine("Enter the number of the save you want to load or press enter to go back:", ConsoleColor.DarkCyan);
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                if (choice < 0 || choice >= allSavesFiles.Count)
                {
                    Utils.WriteLine("Incorrect input. Please enter a number from the list.", ConsoleColor.Red);
                    LoadGame();
                }
                else
                {
                    Console.WriteLine(allSavesFiles[choice]);

                    maze = GameDataWriter.LoadDataFromSaveFile($"GameSaves/{allSavesFiles[choice]}");
                    maze.grid = GameDataWriter.LoadGridFromCsv($"GameSaves/SavedGrid/{Path.ChangeExtension(allSavesFiles[choice].Replace("game", "grid"), ".csv")}");
                    maze.PrintMaze();
                    
                    config.LoadConfig($"GameSaves/{allSavesFiles[choice]}");
                    config = GameDataWriter.config;
                    
                    allmoves = GameDataWriter.allMoves;
                    movesToRegenMaze = GameDataWriter.movesToRegenMaze;
                    playerLastRegenerationPosition = (GameDataWriter.playerLastRegenerationPositionX, GameDataWriter.playerLastRegenerationPositionY);
                    
                    ProcessGameInputTriggers();
                }
            }
            else
            {
                return;
            }
        }

        public static void ProcessGameInputTriggers()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            string keyPressed = keyInfo.Key.ToString();
            string moveInput = keyPressed;

            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                keyPressed = "Ctrl" + keyPressed;
                Console.WriteLine(keyPressed);
                switch (keyPressed)
                {
                    case "CtrlS":
                        Console.WriteLine($"Saving game... {maze.grid}");
                        GameDataWriter.CreateSaveFile(allmoves, movesToRegenMaze, maze.currentPlayerPosition, maze.grid, playerLastRegenerationPosition).GetAwaiter().GetResult();
                        break;
                }
            }

            else 
            {
                bool result;
                (int x, int y) moveDirection;
                allmoves ++;

                switch (moveInput)
                {
                    case "UpArrow" or "W":
                        moveDirection = (maze.currentPlayerPosition.x, maze.currentPlayerPosition.y - 1);
                        break;
                    case "DownArrow" or "S":
                        moveDirection = (maze.currentPlayerPosition.x, maze.currentPlayerPosition.y + 1);
                        break;
                    case "LeftArrow" or "A":
                        moveDirection = (maze.currentPlayerPosition.x - 1, maze.currentPlayerPosition.y);
                        break;
                    case "RightArrow" or "D":
                        moveDirection = (maze.currentPlayerPosition.x + 1, maze.currentPlayerPosition.y);
                        break;                    
                    default:
                        Utils.WriteLine("You want continue game? (yes/y)");
                        Utils.WriteLine("If no, create a save before! And press any key to exit.", ConsoleColor.Red);
                        string choice = Console.ReadLine();
                        List<string> yesAnswers = ["yes", "y", "Yes", "Y"];
                        if (yesAnswers.Contains(choice))
                        {
                            Utils.WriteLine("Just continue press movement key", ConsoleColor.DarkCyan);
                            ProcessGameInputTriggers();
                        }
                        return;
                }

                result = maze.CheckPatchForMove(moveDirection);

                if (result)
                {
                    maze.UpdateGrid(moveDirection);
                    Console.Clear();
                    maze.PrintMaze();
                    CheckRegenerationByGridRange(moveDirection);
                }
 
                if (config.Maze.RegenerationTrigger.Value <= movesToRegenMaze)
                {
                    ReGeneratedMaze reGeneratedMaze = new(config.Maze.Width, config.Maze.Height, (1, 1), (config.Maze.Width - 2, config.Maze.Height - 2), 
                    moveDirection, config.Maze.Symbols.Start, config.Maze.Symbols.Exit, config.Maze.Symbols.Wall, config.Maze.Symbols.Path, 
                    config.Player.Symbol);
                    maze.grid = reGeneratedMaze.grid;
                    Console.Clear();
                    movesToRegenMaze = 0;
                    playerLastRegenerationPosition = moveDirection;
                    reGeneratedMaze.PrintMaze();
                }

                if (maze.CheckPatchForExit(moveDirection))
                {
                    GameDataWriter.SaveGameHistory(allmoves).GetAwaiter().GetResult();;
                    Utils.WriteLine($"You win! Your total moves: {allmoves}!", ConsoleColor.Yellow);
                    Utils.WriteLine("Press enter to continue", ConsoleColor.DarkCyan);
                    Console.ReadLine();
                    return;
                }
            }

            ProcessGameInputTriggers();
        }

        public static void CheckRegenerationByGridRange((int x, int y) currentPlayerPosition)
        {
            int stepsToCheck = (int)Math.Ceiling(config.Maze.RegenerationTrigger.Value / (config.Medium.RegenerationActivationRange * 10));
            int distanceX = Math.Abs(currentPlayerPosition.x - playerLastRegenerationPosition.x);
            int distanceY = Math.Abs(currentPlayerPosition.y - playerLastRegenerationPosition.y);
            int totalDistance = distanceX + distanceY;

            if (totalDistance >= stepsToCheck)
            {
                movesToRegenMaze += 1;
            }
        }
    }
}
