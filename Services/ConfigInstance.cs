using System.Text.Json;
using System.Text.Json.Serialization;

namespace MutableMaze
{
    public class GameConfig
    {
        [JsonPropertyName("maze")]
        public MazeConfig Maze { get; set; }

        [JsonPropertyName("player")]
        public PlayerConfig Player { get; set; }

        [JsonPropertyName("difficulty")]
        public DifficultyConfig Difficulty { get; set; }

        [JsonPropertyName("timer")]
        public TimerConfig Timer { get; set; }

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

            Maze = config.Maze;
            Player = config.Player;
            Difficulty = config.Difficulty;
            Timer = config.Timer;
            Medium = config.Medium;
        }

        public void SaveConfig(string filePath)
        {
            string json = JsonSerializer.Serialize(this, options: new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
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
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
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

    public class TimerConfig
    {
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
        
        [JsonPropertyName("initialTime")]
        public int InitialTime { get; set; }
        
        [JsonPropertyName("timePerMove")]
        public int TimePerMove { get; set; }
    }

    public class MediumConfig
    {
        [JsonPropertyName("RegenerationActivationRange")]
        public double RegenerationActivationRange { get; set; }
        [JsonPropertyName("CountOfRegeneratedStructures")]
        public double CountOfRegeneratedStructures { get; set; }
    }

}
