using System;
using System.Collections.Generic;

namespace MutableMaze
{
    public class Maze
    {
        // Сетка
        public char[,] grid;
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

        // Игрок
        public (int x, int y) currentPlayerPosition;

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
                currentPlayerPosition = playerPosition;
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
        public bool CheckPatchForExit((int x, int y) moveDirection)
        {
            if (moveDirection == endPoint)
            {
                return true;
            }
            return false;
        }
        public bool CheckPatchForMove((int x, int y) moveDirection)
        {
            if (grid[moveDirection.y, moveDirection.x] == pathSymbol)
            {
                return true;
            }
            return false;
        }
        public void UpdateGrid((int x, int y) moveDirection)
        {
            grid[currentPlayerPosition.y, currentPlayerPosition.x] = pathSymbol;
            grid[moveDirection.y, moveDirection.x] = playerSymbol;
            currentPlayerPosition = moveDirection;
        }
    }

public class ReGeneratedMaze
{
    // Сетка
    public char[,] grid;
    private int width;
    private int height;
    private Random random = new Random();
    
    // Фиксированные точки
    public (int x, int y) startPoint;
    public (int x, int y) endPoint;
    public (int x, int y) currentPlayerPosition; // Позиция игрока

    // Символы
    private char wallSymbol;
    private char pathSymbol;
    private char playerSymbol;
    private char startSymbol;
    private char endSymbol;

    public ReGeneratedMaze(int width, int height, (int x, int y) startPoint, (int x, int y) endPoint, (int x, int y) playerPosition,
        char startSymbol, char endSymbol, char wallSymbol, char pathSymbol, char playerSymbol)
    {
        this.width = width % 2 == 0 ? width + 1 : width; // Четная ширина
        this.height = height % 2 == 0 ? height + 1 : height; // Четная высота
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.currentPlayerPosition = playerPosition; // Устанавливаем позицию игрока
        this.startSymbol = startSymbol;
        this.endSymbol = endSymbol;

        this.wallSymbol = wallSymbol;
        this.pathSymbol = pathSymbol;
        this.playerSymbol = playerSymbol;

        grid = new char[height, width];
        InitializeGrid();
        GenerateMaze();

        // Проверка позиции игрока после генерации лабиринта
        if (IsWall(currentPlayerPosition))
        {
            currentPlayerPosition = FindNearestPath(currentPlayerPosition);
        }

        // Устанавливаем символ игрока в сетке
        grid[currentPlayerPosition.y, currentPlayerPosition.x] = playerSymbol; 
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

        // Устанавливаем фиксированные точки
        grid[startPoint.y, startPoint.x] = pathSymbol;
        grid[endPoint.y, endPoint.x] = pathSymbol; // Обеспечиваем проходимость точки выхода
    }

    private void GenerateMaze()
    {
        List<(int x, int y)> activeCells = [startPoint];

        while (activeCells.Count > 0)
        {
            int index = random.Next(activeCells.Count);
            var currentCell = activeCells[index];

            List<(int x, int y)> neighbors = GetNeighbors(currentCell);
            bool addedPath = false;

            foreach (var neighbor in neighbors)
            {
                if (IsWall(neighbor))
                {
                    grid[neighbor.y, neighbor.x] = pathSymbol;
                    RemoveWall(currentCell, neighbor);
                    activeCells.Add(neighbor);
                    addedPath = true;
                }
            }

            if (!addedPath)
            {
                activeCells.RemoveAt(index);
            }
        }

        // Обеспечиваем проходимость стартовой и конечной точек
        grid[startPoint.y, startPoint.x] = pathSymbol;

        // Убедитесь, что выход также проходимый
        if (IsWall(endPoint) != true)
        {
            grid[endPoint.y -1, endPoint.x] = pathSymbol; // Превращаем выход в путь
            grid[endPoint.y, endPoint.x -1] = pathSymbol; // Превращаем выход в путь
        }
    }

    private void RemoveWall((int x, int y) currentCell, (int x, int y) neighbor)
    {
        int wallX = (currentCell.x + neighbor.x) / 2;
        int wallY = (currentCell.y + neighbor.y) / 2;

        grid[wallY, wallX] = pathSymbol;
    }

    private List<(int x, int y)> GetNeighbors((int x, int y) cell)
    {
        List<(int x, int y)> neighbors = new List<(int x, int y)>
        {
            (cell.x + 2, cell.y), // Вправо
            (cell.x - 2, cell.y), // Влево
            (cell.x, cell.y + 2), // Вниз
            (cell.x, cell.y - 2)  // Вверх
        };

        return neighbors.FindAll(n => IsInBounds(n));
    }

    private bool IsInBounds((int x, int y) cell)
    {
        return cell.x > 0 && cell.x < width && cell.y > 0 && cell.y < height;
    }

    private bool IsWall((int x, int y) cell)
    {
        return grid[cell.y, cell.x] == wallSymbol;
    }

    private (int x, int y) FindNearestPath((int x, int y) position)
    {
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
        bool[,] visited = new bool[height, width];

        queue.Enqueue(position);
        
        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();

            if (!IsInBounds(currentPos)) continue;

            if (!IsWall(currentPos))
            {
                return currentPos; // Возвращаем первую найденную проходимую клетку
            }

            visited[currentPos.y, currentPos.x] = true;

            foreach (var neighbor in GetNeighbors(currentPos))
            {
                if (!visited[neighbor.y, neighbor.x])
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return position; // Если не найдено ни одной проходимой клетки (что маловероятно), возвращаем исходную позицию
    }
    public void UpdateGrid((int x, int y) moveDirection)
    {
        grid[currentPlayerPosition.y, currentPlayerPosition.x] = pathSymbol;
        grid[moveDirection.y, moveDirection.x] = playerSymbol;
        currentPlayerPosition = moveDirection;
    }
    public bool CheckPatchForMove((int x, int y) moveDirection)
    {
        if (grid[moveDirection.y, moveDirection.x] == pathSymbol)
        {
            return true;
        }
        return false;
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
                else if (i == currentPlayerPosition.y && j == currentPlayerPosition.x)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(playerSymbol);
                }
                else if (grid[i,j] == wallSymbol)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(wallSymbol);
                }
                else if (grid[i,j] == pathSymbol)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(pathSymbol);
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
}
}