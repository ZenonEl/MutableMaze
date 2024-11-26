using System;

namespace MutableMaze
{
    public class GameLogic
    {
        private static GameConfig config = GameConfig.Instance;
        private static Maze maze = new Maze(config.Maze.Width, config.Maze.Height, config.Maze.Symbols.Wall, config.Maze.Symbols.Path, config.Player.Symbol, config.Maze.Symbols.Start, config.Maze.Symbols.Exit);

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
            string moveInput = Console.ReadKey().Key.ToString();
            bool result;
            (int x, int y) moveDirection;

            switch (moveInput)
            {
                case "CtrlC":
                    return;
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
            ReGeneratedMaze _ = new(config.Maze.Width, config.Maze.Height, (1, 1), (config.Maze.Width - 2, config.Maze.Height - 2), 
            moveDirection, config.Maze.Symbols.Start, config.Maze.Symbols.Exit, config.Maze.Symbols.Wall, config.Maze.Symbols.Path, config.Player.Symbol);
            Console.Clear();
            _.PrintMaze();
            ProcessGameInputTriggers();
        }

        public static bool CheckForExit(Player player, char[,] maze)
        {
            throw new NotImplementedException();
        }
    }
}
