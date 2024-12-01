using System.Text.Json;

namespace MutableMaze
{
    public class GameDataWriter
    {
        public static GameConfig config = GameConfig.Instance;
        public static int movesToRegenMaze = 0;
        public static int allMoves = 0;
        public static int playerLastRegenerationPositionX = -1;
        public static int playerLastRegenerationPositionY = -1;

        static public async Task SaveGameHistory(int allMoves)
        {
            string filePath = $"GameSaves/History/history_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var gameData = new GameSaveData
            {
                allMoves = allMoves,
                config = config
            };

            await WriteJsonToFileAsync(filePath, gameData);
        }

        static public async Task CreateSaveFile(int allMoves, int movesToRegenMaze, (int x, int y) currentPlayerPosition, char[,] grid)
        {
            Console.WriteLine("Creating save file...");
            string filePath = $"GameSaves/saved_game_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            var gameData = new GameSaveData
            {
                allMoves = allMoves,
                movesToRegenMaze = movesToRegenMaze,
                currentPlayerPositionX = currentPlayerPosition.x,
                currentPlayerPositionY = currentPlayerPosition.y,
                config = config
            };
            await WriteJsonToFileAsync(filePath, gameData);
            SaveGridToCsv($"GameSaves/SavedGrid/saved_grid_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv", grid);

        }

        static async Task WriteJsonToFileAsync(string filePath, GameSaveData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(json);
            }
        }
    
    public static void SaveGridToCsv(string filePath, char[,] Grid)
    {
        using (var writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < config.Maze.Height; i++)
            {
                for (int j = 0; j < config.Maze.Width; j++)
                {
                    writer.Write(Grid[i, j]);
                }
                writer.WriteLine();
            }
        }
    }

    public static ReGeneratedMaze LoadDataFromSaveFile(string filePath)
    {
        int currentPlayerPositionX = 0;
        int currentPlayerPositionY = 0;

        string json = File.ReadAllText(filePath);
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            JsonElement root = doc.RootElement;

            allMoves = root.GetProperty("allMoves").GetInt32();
            movesToRegenMaze = root.GetProperty("movesToRegenMaze").GetInt32();
            currentPlayerPositionX = root.GetProperty("currentPlayerPositionX").GetInt32();
            currentPlayerPositionY = root.GetProperty("currentPlayerPositionY").GetInt32();
            playerLastRegenerationPositionX = root.GetProperty("PlayerLastRegenerationPositionX").GetInt32();
            playerLastRegenerationPositionY = root.GetProperty("PlayerLastRegenerationPositionY").GetInt32();

            config = root.GetProperty("config").Deserialize<GameConfig>();

            Console.WriteLine($"All Moves: {allMoves}");
            Console.WriteLine($"Moves To Regen Maze: {movesToRegenMaze}");
            Console.WriteLine($"Current Player Position: ({currentPlayerPositionX}, {currentPlayerPositionY})");
            Console.WriteLine($"Config: {config.Maze.Width}, {config.Maze.Height}, {config.Maze.Symbols.Start}, {config.Maze.Symbols.Exit}, {config.Maze.Symbols.Wall}, {config.Maze.Symbols.Path}, {config.Player.Symbol}");
            Console.WriteLine($"Last Regeneration Position: {playerLastRegenerationPositionX}, {playerLastRegenerationPositionY}");
        }
        return new ReGeneratedMaze(config.Maze.Width, config.Maze.Height, (1, 1), (config.Maze.Width - 2, config.Maze.Height - 2), 
                        (currentPlayerPositionX, currentPlayerPositionY), config.Maze.Symbols.Start, config.Maze.Symbols.Exit, config.Maze.Symbols.Wall, 
                        config.Maze.Symbols.Path, config.Player.Symbol);
    }

    public static char[,] LoadGridFromCsv(string filePath)
    {
        char[,] Grid = new char[config.Maze.Height, config.Maze.Width];
        using (var reader = new StreamReader(filePath))
        {
            for (int i = 0; i < config.Maze.Height; i++)
            {
                var line = reader.ReadLine();
                if (line == null) break;

                for (int j = 0; j < config.Maze.Width; j++)
                {
                    Grid[i, j] = line[j];
                }
            }
        }
        return Grid;
    
    }

    public class GameSaveData
    {
        public int allMoves { get; set; }
        public int movesToRegenMaze { get; set; }
        public int currentPlayerPositionX { get; set; }
        public int currentPlayerPositionY { get; set; }
        public GameConfig config { get; set; }
    }
    }
}
