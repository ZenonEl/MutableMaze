

namespace MutableMaze
{
    public class GameLogic
    {
        public static List<string> allSavesFiles = []; 
        private static GameConfig config = GameConfig.Instance;
        private static ReGeneratedMaze maze = new(config.Maze.Width, config.Maze.Height, (1, 1), (config.Maze.Width - 2, config.Maze.Height - 2), 
                        (config.Player.StartX, config.Player.StartY), config.Maze.Symbols.Start, config.Maze.Symbols.Exit, config.Maze.Symbols.Wall, 
                        config.Maze.Symbols.Path, config.Player.Symbol);

        private static int movesToRegenMaze = 0;
        private static int allmoves = 0;

        public static void StartGame()
        {
            maze.PrintMaze();
            ProcessGameInputTriggers();
        }

        public static void LoadGame()
        {

            string[] jsonFiles = Directory.GetFiles("GameSaves", "*.json");
            int index = 0;
            int choice;
            string filename;
            Console.WriteLine("Your saves:");
            foreach (string file in jsonFiles)
            {
                filename = Path.GetFileName(file);
                Console.WriteLine($"{index}. {filename}"); 
                allSavesFiles.Add(filename);
                index++;
            }
            Console.WriteLine("Enter the number of the save you want to load:");
            choice = int.Parse(Console.ReadLine());
            Console.WriteLine(allSavesFiles[choice]);

            config.LoadConfig($"GameSaves/{allSavesFiles[choice]}");
            maze = GameDataWriter.LoadDataFromSaveFile($"GameSaves/{allSavesFiles[choice]}");
            maze.grid = GameDataWriter.LoadGridFromCsv($"GameSaves/SavedGrid/{Path.ChangeExtension(allSavesFiles[choice].Replace("game", "grid"), ".csv")}");
            config = GameDataWriter.config;
            maze.PrintMaze();
            ProcessGameInputTriggers();
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
                        GameDataWriter.CreateSaveFile(0, allmoves, movesToRegenMaze, maze.currentPlayerPosition, maze.grid).GetAwaiter().GetResult();
                        break;
                }
            }
            else 
            {
                bool result;
                (int x, int y) moveDirection;
                allmoves += 1;

                switch (moveInput)
                {
                    case "UpArrow":
                        moveDirection = (maze.currentPlayerPosition.x, maze.currentPlayerPosition.y - 1);
                        break;
                    case "DownArrow":
                        moveDirection = (maze.currentPlayerPosition.x, maze.currentPlayerPosition.y + 1);
                        break;
                    case "LeftArrow":
                        moveDirection = (maze.currentPlayerPosition.x - 1, maze.currentPlayerPosition.y);
                        break;
                    case "RightArrow":
                        moveDirection = (maze.currentPlayerPosition.x + 1, maze.currentPlayerPosition.y);
                        break;
                    default:
                        return;
                }

                result = maze.CheckPatchForMove(moveDirection);
                if (result)
                {
                    maze.UpdateGrid(moveDirection);
                    Console.Clear();
                    maze.PrintMaze();
                }
                switch (config.Maze.RegenerationTrigger.Type)
                {
                    case "moves":
                    if (config.Maze.RegenerationTrigger.Value == movesToRegenMaze)
                    {
                        ReGeneratedMaze reGeneratedMaze = new(config.Maze.Width, config.Maze.Height, (1, 1), (config.Maze.Width - 2, config.Maze.Height - 2), 
                        moveDirection, config.Maze.Symbols.Start, config.Maze.Symbols.Exit, config.Maze.Symbols.Wall, config.Maze.Symbols.Path, config.Player.Symbol);
                        maze.grid = reGeneratedMaze.grid;
                        Console.Clear();
                        movesToRegenMaze = 0;
                        reGeneratedMaze.PrintMaze();
                    }
                    movesToRegenMaze += 1;
                    break;
                }
                if (maze.CheckPatchForExit(moveDirection))
                {
                    GameDataWriter.SaveGameHistory(0, allmoves).GetAwaiter().GetResult();;
                    Console.WriteLine($"You win! Your moves: {allmoves} Your time: Таймер");
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    return;
                }
            }

            ProcessGameInputTriggers();
        }

        public static bool CheckForExit(Player player, char[,] maze)
        {
            throw new NotImplementedException();
        }
    }
}
