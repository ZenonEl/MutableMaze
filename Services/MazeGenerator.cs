using System;
using System.Collections.Generic;

namespace MutableMaze
{
    public class Maze
    {
        // Сетка
        private char[,] grid;
        private int width;
        private int height;
        private Random random = new Random();
        private (int x, int y) startPoint = (1, 1);
        private (int x, int y) endPoint;

        // Символы
        private char wallSymbol;
        private char pathSymbol;
        private char playerSymbol;
        private char startSymbol;
        private char endSymbol;

        public Maze(int width = 21, int height = 21, char wallSymbol = '#', char pathSymbol = '.', char playerSymbol = 'P', char startSymbol = 'S', char endSymbol = 'E')
        {
            // Проверка  четности
            this.width = width % 2 == 0 ? width + 1 : width;
            this.height = height % 2 == 0 ? height + 1 : height;
            grid = new char[height, width];
            endPoint = (width - 2, height - 2);

            // Инициализация символов
            this.wallSymbol = wallSymbol;
            this.pathSymbol = pathSymbol;
            this.playerSymbol = playerSymbol;
            this.startSymbol = startSymbol;
            this.endSymbol = endSymbol;

            InitializeGrid();
            GenerateMaze(startPoint.x, startPoint.y);
            PlacePlayer();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grid[i, j] = wallSymbol; // Заполняем стены
                }
            }
        }

        private void GenerateMaze(int x, int y)
        {
            grid[y, x] = pathSymbol; // Делаем текущую клетку проходимой

            // Список направлений для случайного выбора
            var directions = new (int dx, int dy)[]
            {
                (2, 0), (-2, 0), (0, 2), (0, -2)
            };

            Shuffle(directions); // Перемешиваем направления

            foreach (var (dx, dy) in directions)
            {
                int newX = x + dx;
                int newY = y + dy;

                // Проверяем границы и наличие стены
                if (newX > 0 && newX < width && newY > 0 && newY < height && grid[newY, newX] == wallSymbol)
                {
                    // Убираем стену между текущей и новой клеткой
                    grid[y + dy / 2, x + dx / 2] = pathSymbol;
                    GenerateMaze(newX, newY); // Рекурсивно генерируем дальше
                }
            }
        }

        private void Shuffle((int dx, int dy)[] directions)
        {
            for (int i = directions.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = directions[i];
                directions[i] = directions[j];
                directions[j] = temp;
            }
        }

        public void PrintMaze()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == startPoint.y && j == startPoint.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(startSymbol);
                    }
                    else if (i == endPoint.y && j == endPoint.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(endSymbol);
                    }
                    else if (grid[i, j] == wallSymbol)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(wallSymbol);
                    }
                    else if (grid[i, j] == pathSymbol)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(pathSymbol);
                    }
                    else if (grid[i, j] == playerSymbol)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(playerSymbol);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
                Console.ResetColor();
            }
        }

        private void PlacePlayer()
        {
            List<(int x, int y)> possiblePositions = new List<(int x, int y)>();

            CheckPosition(startPoint.x + 1, startPoint.y, possiblePositions); // Справа
            CheckPosition(startPoint.x - 1, startPoint.y, possiblePositions); // Слева
            CheckPosition(startPoint.x, startPoint.y + 1, possiblePositions); // Снизу
            CheckPosition(startPoint.x, startPoint.y - 1, possiblePositions); // Сверху

            if (possiblePositions.Count > 0)
            {
                var playerPosition = possiblePositions[0];
                grid[playerPosition.y, playerPosition.x] = playerSymbol;
            }
        }

        private void CheckPosition(int x, int y, List<(int x, int y)> positions)
        {
            if (x >= 0 && x < width && y >= 0 && y < height && grid[y, x] == pathSymbol)
            {
                positions.Add((x, y)); // Добавляем позицию в список возможных позиций для игрока
            }
        }
    }
}
