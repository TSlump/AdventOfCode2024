namespace AdventOfCode24.Days.Day15;

public class Day15
{
    public static void RunPartOne()
    {
        var filePath = "Days/Day15/Day15Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }


        var breaker = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == "")
            {
                breaker = i;
            }
        }
        
        (int y, int x) robotCoords = (-1, -1);
        
        var gridArray = input.Take(breaker).ToArray();

        var grid = new string[gridArray.Length, gridArray[0].Length];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = gridArray[i][j].ToString();

                if (gridArray[i][j] == '@')
                {
                    robotCoords = (i, j);
                }
            }
        }
        
        var movementsArray = input.Skip(breaker + 1).ToArray();

        var movements = "";
        
        foreach (var movement in movementsArray)
        {
            movements += movement;
        }

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i,j]);
            }
            Console.WriteLine();
        }
        
        Console.WriteLine(movements);
        
        
        // # is wall
        // O is box
        // X is immovable box
        // @ is robot
        // . is space
        
        SearchGridFromCorners(grid);
        
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i,j]);
            }
            Console.WriteLine();
        }
        
        Console.WriteLine($"Grid Size: {grid.GetLength(0)}, {grid.GetLength(1)}");
        
        Console.WriteLine($"Robot Coordinates: {robotCoords.y}, {robotCoords.x}");

        foreach (var movement in movements)
        {
            var direction = GetDirection(movement);
            
            var boxesPushed = 0;
            
            //Console.WriteLine($"Direction: {direction.y}, {direction.x}, Item in that direction: {grid[robotCoords.y + (boxesPushed + 1) * direction.y, robotCoords.x + (boxesPushed + 1) * direction.x]}");

            while (grid[robotCoords.y + (boxesPushed + 1) * direction.y, robotCoords.x + (boxesPushed + 1) * direction.x] == "O")
            {
                
                //Console.WriteLine(($"Looking +{boxesPushed}: {grid[robotCoords.y + (boxesPushed + 1) * direction.y, robotCoords.x + (boxesPushed + 1) * direction.x]}"));
                boxesPushed += 1;
            }
            
            //Console.WriteLine(($"Blocked by: {grid[robotCoords.y + (boxesPushed + 1) * direction.y, robotCoords.x + (boxesPushed + 1) * direction.x]}"));
    
            if (grid[robotCoords.y + (boxesPushed + 1) * direction.y, robotCoords.x + (boxesPushed + 1) * direction.x] == ".")
            {
                grid[robotCoords.y, robotCoords.x] = ".";
                grid[robotCoords.y + direction.y, robotCoords.x + direction.x] = "@";
                
                if (boxesPushed > 0)
                {
                    for (int i = boxesPushed + 1; i > 1; i--)
                    {
                        grid[robotCoords.y + i * direction.y, robotCoords.x + i * direction.x] = "O";

                        if (IsImmovable(grid, (robotCoords.y + i * direction.y, robotCoords.x + i * direction.x)))
                        {
                            grid[robotCoords.y + i * direction.y, robotCoords.x + i * direction.x] = "X";
                        }
                    }
                }
                
                robotCoords.y += direction.y;
                robotCoords.x += direction.x;
            }
            else
            {
                //Console.WriteLine("Blocked");
            }
            
            // for (int i = 0; i < grid.GetLength(0); i++)
            // {
            //     for (int j = 0; j < grid.GetLength(1); j++)
            //     {
            //         Console.Write(grid[i,j]);
            //     }
            //     Console.WriteLine();
            // }
        }


        long sumGPS = 0;
        
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i,j]);

                if (grid[i, j] == "O" || grid[i, j] == "X")
                {
                    sumGPS += 100 * i + j;
                }
            }
            Console.WriteLine();
        }
        
        
        Console.WriteLine($"GPS: {sumGPS}");
        
    }
    

    public static bool IsImmovable(string[,] grid, (int y, int x) position)
    {
        var directions = new (int y, int x)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };

        for (int i = 0; i < 4; i++)
        {
            if ((grid[position.y + directions[i].y, position.x + directions[i].x] == "X" ||
                 grid[position.y + directions[i].y, position.x + directions[i].x] == "#") &&
                (grid[position.y + directions[(i + 1) % 4].y, position.x + directions[(i + 1) % 4].x] == "X" ||
                grid[position.y + directions[(i + 1) % 4].y, position.x + directions[(i + 1) % 4].x] == "#"))
            {
                return true;
            }
        }

        return false;
    }

    public static void SearchGridFromCorners(string[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        var visited = new HashSet<(int, int)>();
        var directions = new (int y, int x)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        
        var queue = new Queue<(int, int)>();
        queue.Enqueue((1, 1));
        queue.Enqueue((1, cols - 2));
        queue.Enqueue((rows - 2, 1));
        queue.Enqueue((rows - 2, cols - 2));
        
        visited.Add((1,1));
        visited.Add((1, cols - 2));
        visited.Add((rows - 2, 1));
        visited.Add((rows - 2, cols - 2));
        
        while (queue.Count > 0)
        {
            var (y, x) = queue.Dequeue();

            if (grid[y, x] == "O" && IsImmovable(grid, (y, x)))
            //if (grid[y, x] == "O")
            {
                grid[y, x] = "X";
            }
            
            foreach (var direction in directions)
            {
                
                if (!visited.Contains((y + direction.y, x + direction.x)) && grid[y + direction.y, x + direction.x] != "#")
                {
                    queue.Enqueue((y + direction.y, x + direction.x));
                    
                    visited.Add((y + direction.y, x + direction.x));
                }
            }
        }
    }

    public static (int y, int x) GetDirection(char direction)
    {
        return direction switch
        {
            '>' => (0, 1),
            '^' => (-1, 0),
            '<' => (0, -1),
            'v' => (1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}