using System.Text.Json.Serialization;

namespace MutableMaze
{
public class ReGeneratedMaze
{
    // Сетка
    [JsonIgnore]
    public char[,] grid;
    public int width;
    public int height;
    [JsonIgnore]
    public Random random = new Random();
    
    // Фиксированные точки
    public (int x, int y) startPoint;
    public (int x, int y) endPoint;
    public (int x, int y) currentPlayerPosition;

    // Символы
    public char wallSymbol;
    public char pathSymbol;
    public char playerSymbol;
    public char startSymbol;
    public char endSymbol;

    public ReGeneratedMaze(int width, int height, (int x, int y) startPoint, (int x, int y) endPoint, (int x, int y) playerPosition,
        char startSymbol, char endSymbol, char wallSymbol, char pathSymbol, char playerSymbol)
    {
        this.width = width % 2 == 0 ? width + 1 : width; // Четная ширина
        this.height = height % 2 == 0 ? height + 1 : height; // Четная высота
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        currentPlayerPosition = playerPosition; // Устанавливаем позицию игрока
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
    public bool CheckPatchForExit((int x, int y) moveDirection)
    {
        if (moveDirection == endPoint)
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