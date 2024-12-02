namespace MutableMaze
{
    class Program
    {
        private static GameConfig config = GameConfig.Instance;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Mutable Maze!");
            config.LoadConfig("Config/config.json");
            GameMenu.PrintMenu();
            GameMenu.GetChoiceInput();
        }
    }
}
