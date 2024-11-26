using System;

namespace MutableMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Mutable Maze!");
            GameConfig config = GameConfig.Instance;
            config.LoadConfig("Config/config.json");
            Maze maze = new Maze(config.Maze.Width, config.Maze.Height, config.Maze.Symbols.Wall, config.Maze.Symbols.Path, config.Player.Symbol, config.Maze.Symbols.Start, config.Maze.Symbols.Exit);
            maze.PrintMaze();
            GameLogic.StartGame();
        }
    }
}
