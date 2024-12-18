namespace AdventOfCode2024.Days.Day18;

public class Day18
{
    public static void Run()
    {
        var filePath = "Days/Day18/Day18Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        (int x, int y) gridSize = (71, 71);
        
        var grid = new string[gridSize.x, gridSize.y];

        for (var y = 0; y < gridSize.y; y++)
        {
            for (var x = 0; x < gridSize.x; x++)
            {
                grid[y, x] = ".";
            }
        }

        for (var i = 0; i < 1024; i++)
        {
            var line = input[i];
            
            var coords = line.Split(',').Select(int.Parse).ToArray();
            
            grid[coords[1], coords[0]] = "#";
        }

        for (var y = 0; y < gridSize.y; y++)
        {
            for (var x = 0; x < gridSize.x; x++)
            {
                Console.Write(grid[y, x]);
            }
            Console.WriteLine();
        }
        
        var solution = BreadthFirstSearch(grid, gridSize);
        
        Console.WriteLine($"PartOne: \n Solution length: {solution.Count}");

        for (var i = 1024; i < input.Length; i++)
        {
            var line = input[i];
            
            var coords = line.Split(',').Select(int.Parse).ToArray();
            
            grid[coords[1], coords[0]] = "#";
            
            solution = BreadthFirstSearch(grid, gridSize);

            if (solution.Count == 0)
            {
                Console.WriteLine($"PartTwo: \n Blocking byte: {coords[1]}, {coords[0]}");
                break;
            }
        }
    }

    public static List<(int x, int y)> BreadthFirstSearch(string[,] grid, (int x, int y) gridSize)
    {
        var print = false;
        
        Queue<((int y, int x) coords, List<(int, int)> path)> queue = new();

        var visited = new bool[gridSize.y, gridSize.x];

        visited[0, 0] = true;

        queue.Enqueue(((0, 0), []));
        
        (int x, int y) finish = (gridSize.x - 1, gridSize.y - 1);

        while (queue.Count > 0)
        {
            var (currentCoords, currentPath) = queue.Dequeue();

            if (print)
            {
                Console.WriteLine($"(Node: {currentCoords.y}, {currentCoords.x})");
            }
            
            var amendedPath = currentPath.ToList();
            amendedPath.Add((currentCoords.x, currentCoords.y));

            if (currentCoords == finish)
            {
                return currentPath;
            }
            
            var directions = new (int y, int x)[] {(0, 1), (0, -1), (1, 0), (-1, 0)};

            foreach (var direction in directions)
            {
                try
                {
                    if (print)
                    {
                        Console.WriteLine($"Looking at {direction.y}, {direction.x}, seeing: {grid[currentCoords.y + direction.y, currentCoords.x + direction.x]}");
                    }
                    
                    
                    if (!visited[currentCoords.y + direction.y, currentCoords.x + direction.x] &&
                        grid[currentCoords.y + direction.y, currentCoords.x + direction.x] != "#")
                    {
                        if (print)
                        {
                            Console.WriteLine($"Added {currentCoords.y + direction.y}, {currentCoords.x + direction.x} to queue");
                        }
                        
                        visited[currentCoords.y + direction.y, currentCoords.x + direction.x] = true;

                        queue.Enqueue(((currentCoords.y + direction.y, currentCoords.x + direction.x), amendedPath));
                    }
                }
                catch
                {
                    //Ignored
                }
                
            }
        }

    return [];
}
}