namespace AdventOfCode2024.Days.Day12;

public class Day12
{
    public static void Run()
    {
        var filePath = "Days/Day12/Day12Input.txt";

        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        var grid = new string[input.Length, input[0].Length];

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                grid[y, x] = input[y][x].ToString();
            }
        }

        for (var y = 0; y < input.Length; y++)
        {
            Console.WriteLine(string.Join("", input[y]));
        }
        
        var regions = new List<(string, List<(int, int)>)>();
        
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (!regions
                    .Where(entry => entry.Item1 == grid[y, x])
                    .Any(entry => entry.Item2.Contains((y, x))))
                {
                    regions.Add((grid[y, x], BreadthFirstSearch(grid, (y, x))));
                }
            }
        }

        var initialCost = 0.0;
        var bulkCost = 0.0;

        foreach (var region in regions)
        {
            var perimeter = ComputePerimeter(region.Item2);
            
            initialCost += perimeter * region.Item2.Count;

            var corners = CountTotalCorners(region.Item2);
            
            bulkCost += corners * region.Item2.Count;
            
            Console.WriteLine($"letter: {region.Item1}, area: {region.Item2.Count}, corners: {corners}");
        }
        
        Console.WriteLine($"Total initial cost: {initialCost}");
        
        Console.WriteLine($"Total bulk cost: {bulkCost}");
    }

    public static List<(int, int)> BreadthFirstSearch(string[,] grid, (int y, int x) start)
    {
        var queue = new Queue<(int, int)>();
        
        var visited = new List<(int, int)>();
        
        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            
            var neighbours = GetNeighbours(grid, currentPos);

            foreach (var neighbour in neighbours)
            {
                if (!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }
        
        return visited;
    }

    public static List<(int, int)> GetNeighbours(string[,] grid, (int y, int x) pos)
    {
        var neighbours = new List<(int, int)>();
        
        var directions = new List<(int, int)>{ (1, 0), (-1, 0), (0, 1), (0, -1) };

        foreach (var direction in directions)
        {
            try
            {
                if (grid[pos.y, pos.x] == grid[pos.y + direction.Item1, pos.x + direction.Item2])
                {
                    neighbours.Add((pos.y + direction.Item1, pos.x + direction.Item2));
                }
            }
            catch (Exception)
            {
                continue;
            }
        }

        return neighbours;
    }

    public static double ComputePerimeter(List<(int, int)> plots)
    {
        var perimeter = 0.0;
        
        var directions = new List<(int, int)>{ (1, 0), (-1, 0), (0, 1), (0, -1) };

        foreach (var plot in plots)
        {
            foreach (var direction in directions)
            {
                if (!plots.Contains((plot.Item1 + direction.Item1, plot.Item2 + direction.Item2)))
                {
                    perimeter += 1;
                }
            }
        }

        return perimeter;
    }
    
    public static double CountTotalCorners(List<(int, int)> plots)
    {
        var cornerCounter = 0.0;

        foreach (var plot in plots)
        {
            cornerCounter += CountPlotCorners(plots, plot);
        }

        return cornerCounter;
    }

    public static int CountPlotCorners(List<(int, int)> region, (int, int) plot)
    {
        var cornerCounter = 0;
        
        var directions = new List<(int, int)>{ (-1, 0), (0, 1), (1, 0), (0, -1) };

        for (int i = 0; i < 4; i++)
        {
            if (!region.Contains((plot.Item1 + directions[i].Item1, plot.Item2 + directions[i].Item2)) &&
                !region.Contains((plot.Item1 + directions[(i + 1) % 4].Item1, plot.Item2 + directions[(i + 1) % 4].Item2)))
            {
                cornerCounter += 1;
            }

            var diagonalDirection = (directions[i].Item1 + directions[(i + 1) % 4].Item1,
                directions[i].Item2 + directions[(i + 1) % 4].Item2);

            if (region.Contains((plot.Item1 + directions[i].Item1, plot.Item2 + directions[i].Item2)) &&
                region.Contains((plot.Item1 + directions[(i + 1) % 4].Item1, plot.Item2 + directions[(i + 1) % 4].Item2)) &&
                !region.Contains((plot.Item1 + diagonalDirection.Item1, plot.Item2 + diagonalDirection.Item2)))
            {
                cornerCounter += 1;
            }
            
        }
        
        return cornerCounter;
    } 
}