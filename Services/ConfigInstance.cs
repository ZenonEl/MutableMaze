using System.Text.Json;
using System.Text.Json.Serialization;

namespace MutableMaze
{
    public class GameConfig
    {
        [JsonPropertyName("color")]
        public ColorConfig Color { get; set; }
        [JsonPropertyName("maze")]
        public MazeConfig Maze { get; set; }

        [JsonPropertyName("player")]
        public PlayerConfig Player { get; set; }

        [JsonPropertyName("difficulty")]
        public DifficultyConfig Difficulty { get; set; }

        [JsonPropertyName("medium")]
        public MediumConfig Medium { get; set; }

        private static GameConfig instance;

        [JsonConstructor]
        private GameConfig() { }

        public static GameConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameConfig();
                }
                return instance;
            }
        }

        public void LoadConfig(string filePath)
        {
            string json = File.ReadAllText(filePath);
            GameConfig config = JsonSerializer.Deserialize<GameConfig>(json);

            Color = config.Color;
            Maze = config.Maze;
            Player = config.Player;
            Difficulty = config.Difficulty;
            Medium = config.Medium;
        }

        public void SaveConfig(string filePath)
        {
            string json = JsonSerializer.Serialize(this, options: new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
    public class ColorConfig
    {
        [JsonPropertyName("coloredEnabled")]
        public bool ColoredEnabled { get; set; }

        [JsonPropertyName("defaultColor")]
        public string DefaultColor { get; set; }
    }
    public class MazeConfig
    {
        [JsonPropertyName("width")]
        public int Width { get; set; }
        
        [JsonPropertyName("height")]
        public int Height { get; set; }
        
        [JsonPropertyName("symbols")]
        public MazeSymbols Symbols { get; set; }
        
        [JsonPropertyName("regenerationTrigger")]
        public RegenerationTrigger RegenerationTrigger { get; set; }

    }

    public class MazeSymbols
    {
        [JsonPropertyName("wall")]
        public char Wall { get; set; }
        
        [JsonPropertyName("path")]
        public char Path { get; set; }
        
        [JsonPropertyName("start")]
        public char Start { get; set; }
        
        [JsonPropertyName("exit")]
        public char Exit { get; set; }
    }

    public class RegenerationTrigger
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    public class PlayerConfig
    {
        [JsonPropertyName("startX")]
        public int StartX { get; set; }
        
        [JsonPropertyName("startY")]
        public int StartY { get; set; }
        
        [JsonPropertyName("symbol")]
        public char Symbol { get; set; }
    }

    public class DifficultyConfig
    {
        [JsonPropertyName("level")]
        public string Level { get; set; }
    }

    public class MediumConfig
    {
        [JsonPropertyName("RegenerationActivationRange")]
        public double RegenerationActivationRange { get; set; }
        [JsonPropertyName("CountOfRegeneratedStructures")]
        public double CountOfRegeneratedStructures { get; set; }
    }

}
