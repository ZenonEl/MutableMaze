using System;

namespace MutableMaze
{
    public class GameLogic
    {
        private static GameConfig config = GameConfig.Instance;
        private static Maze maze = new Maze(config.Maze.Width, config.Maze.Height, config.Maze.Symbols.Wall, config.Maze.Symbols.Path, config.Player.Symbol, config.Maze.Symbols.Start, config.Maze.Symbols.Exit);
        private static int movesToRegenMaze = 0;
        private static int allmoves = 0;

        public static void StartGame()
        {
            Console.Clear();
            maze.PrintMaze();
            ProcessGameInputTriggers();
        }

        public static void LoadGame()
        {
            Console.WriteLine("Loading game...");
        }

        public static void ProcessGameInputTriggers()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true для скрытия нажатой клавиши

            // Получаем символ нажатой клавиши
            string keyPressed = keyInfo.Key.ToString();
            string moveInput = keyPressed;

            // Проверяем, были ли нажаты модификаторы
            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                keyPressed = "Ctrl" + keyPressed; // Добавляем информацию о Ctrl
                Console.WriteLine(keyPressed);
                switch (keyPressed)
                {
                    case "CtrlS":
                        Console.WriteLine("Saving game...");
                        GameDataWriter.CreateSaveFile().GetAwaiter().GetResult();
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
