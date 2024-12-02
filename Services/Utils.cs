namespace MutableMaze
{
    public static class Utils
    {
        private static GameConfig config = GameConfig.Instance;

        public static void Write(char text, ConsoleColor color = ConsoleColor.White)
        {
            setColor(color);
            Console.Write(text);
            Console.ResetColor();
        }
        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
        {
            setColor(color);
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void setColor(ConsoleColor color)
        {
            try
            {
            if (config.Color.ColoredEnabled)
            {
                Console.ForegroundColor = color;
            }
            else
            {
                switch (config.Color.DefaultColor)
                {
                    case "White":
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "Red":
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case "Green":
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "Yellow":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case "Blue":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case "Magenta":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case "Cyan":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                }
            }
            }
            catch (Exception ex) {}
        }
    }
}