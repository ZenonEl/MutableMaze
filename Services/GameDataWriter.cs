using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MutableMaze
{
    public class GameDataWriter
    {
        private int Timer { get; set; }
        private int AllMoves { get; set; }
        private static GameConfig config = GameConfig.Instance;

        // Метод для сохранения истории игры
        static public async Task SaveGameHistory(int timer, int allMoves)
        {
            string filePath = $"GameSaves/History/history_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Создаем объект для сохранения данных
            var gameData = new GameHistoryData
            {
                Timer = timer,
                AllMoves = allMoves,
                Config = config // Предполагается, что config сериализуемый
            };

            await WriteJsonToFileAsync(filePath, gameData);
        }

        // Метод для записи данных в JSON файл
        static async Task WriteJsonToFileAsync(string filePath, GameHistoryData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; // Для форматирования
            string json = JsonSerializer.Serialize(data, options);

            // Используем using для безопасного открытия файла
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(json); // Асинхронная запись строки
            }
        }

        static public async Task CreateSaveFile()
        {
            Console.WriteLine("Creating save file...");
            string filePath = $"GameSaves/saved_game_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
        }
    }


public interface GameData {}

    public class GameHistoryData : GameData
    {
        public int Timer { get; set; }
        public int AllMoves { get; set; }
        public GameConfig Config { get; set; }
    }

    public class GameSaveData : GameData  // Для отрисовки надо: грид позиция игрока точки старта выхода и символы 
    {
        public int Timer { get; set; }
        public int AllMoves { get; set; }
        // Сетка
        public char[,] grid;
        private int width;
        private int height;
        
        // Фиксированные точки
        public (int x, int y) startPoint;
        public (int x, int y) endPoint;
        public (int x, int y) currentPlayerPosition;

        // Символы
        private char wallSymbol;
        private char pathSymbol;
        private char playerSymbol;
        private char startSymbol;
        private char endSymbol;
        }

    // Класс для хранения данных игры

}
