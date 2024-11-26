using System;
using System.IO;
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

            this.Maze = config.Maze;
            this.Player = config.Player;
            this.Difficulty = config.Difficulty;
            this.Timer = config.Timer;
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
}