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

    public static void RunPartTwo()
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

        var grid = new string[gridArray.Length, 2 * gridArray[0].Length];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1) / 2; j++)
            {
                if (gridArray[i][j] == '@')
                {
                    robotCoords = (i, 2 * j);
                    grid[i, 2 * j] = "@";
                    grid[i, 2 * j + 1] = ".";
                }
                else if (gridArray[i][j] == '#' || gridArray[i][j] == '.')
                {
                    grid[i, 2 * j] = gridArray[i][j].ToString();
                    grid[i, 2 * j + 1] = gridArray[i][j].ToString();
                }
                else if (gridArray[i][j] == 'O')
                {
                    grid[i, 2 * j] = "[";
                    grid[i, 2 * j + 1] = "]";
                }
            }
        }
        
        var movementsArray = input.Skip(breaker + 1).ToArray();

        var movements = movementsArray.Aggregate("", (current, movement) => current + movement);

        Console.WriteLine(movements);

        foreach (var direction in movements.Select(GetDirection))
        {
            Console.WriteLine($"Direction: {direction.y}, {direction.x}");

            switch (grid[robotCoords.y + direction.y, robotCoords.x + direction.x])
            {
                case ".":
                    grid[robotCoords.y, robotCoords.x] = ".";
                    grid[robotCoords.y + direction.y, robotCoords.x + direction.x] = "@";
                    
                    robotCoords.y += direction.y;
                    robotCoords.x += direction.x;
                    continue;
                case "X":
                case "#":
                    continue;
            }

            var depth = 0;

            var listOfFutureSpaces = new List<HashSet<(int y, int x)>> { new() };

            listOfFutureSpaces[0].Add((robotCoords.y, robotCoords.x));

            var isMovable = true;
            
            while (listOfFutureSpaces[depth].Count > 0)
            {
                listOfFutureSpaces.Add([]);
                
                depth++;
                
                foreach (var futureSpace in listOfFutureSpaces[depth - 1])
                {
                    if (grid[futureSpace.y + direction.y, futureSpace.x + direction.x] == "#" || grid[futureSpace.y + direction.y, futureSpace.x + direction.x] == "X")
                    {
                        Console.WriteLine("Wall found, breaking");
                        isMovable = false;
                        break;
                    }
                    
                    Console.WriteLine($"Looking at: {grid[futureSpace.y + direction.y, futureSpace.x + direction.x]}");

                    if (grid[futureSpace.y + direction.y, futureSpace.x + direction.x] == "[")
                    {
                        Console.WriteLine(" [ added");
                        listOfFutureSpaces[depth].Add((futureSpace.y + direction.y, futureSpace.x + direction.x));

                        if (IsVerticleDirection(direction))
                        {
                            Console.WriteLine($"Found vertical direction {direction.y}, {direction.x}");
                            listOfFutureSpaces[depth].Add((futureSpace.y + direction.y, futureSpace.x + direction.x + 1));
                        }
                    }
                    
                    if (grid[futureSpace.y + direction.y, futureSpace.x + direction.x] == "]")
                    {
                        Console.WriteLine(" ] added ");
                        listOfFutureSpaces[depth].Add((futureSpace.y + direction.y, futureSpace.x + direction.x));

                        if (IsVerticleDirection(direction))
                        {
                            Console.WriteLine($"Found vertical direction {direction.y}, {direction.x}");
                            listOfFutureSpaces[depth].Add((futureSpace.y + direction.y, futureSpace.x + direction.x - 1));
                        }
                    }
                    
                }

                if (!isMovable)
                {
                    break;
                }
                
                Console.WriteLine($"Current list of future spaces: {string.Join(", ", listOfFutureSpaces[depth])}");
            }

            for (int i = 0; i < listOfFutureSpaces.Count; i++)
            {
                Console.WriteLine($"Future spaces: (depth {i})  {string.Join(", ", listOfFutureSpaces[i])}");
            }

            if (isMovable)
            {
                for (int i = listOfFutureSpaces.Count - 1; i >= 0; i--)
                {
                    foreach (var futureSpace in listOfFutureSpaces[i])
                    {
                        grid[futureSpace.y + direction.y, futureSpace.x + direction.x] = grid[futureSpace.y, futureSpace.x];
                        grid[futureSpace.y, futureSpace.x] = ".";
                    }
                }
                
                grid[robotCoords.y, robotCoords.x] = ".";
                grid[robotCoords.y + direction.y, robotCoords.x + direction.x] = "@";
                
                robotCoords.y += direction.y;
                robotCoords.x += direction.x;
            }
        }
        
        long sumGPS = 0;
        
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i,j]);

                if (grid[i, j] == "[")
                {
                    sumGPS += 100 * i + j;
                }
            }
            Console.WriteLine();
        }
        
        
        Console.WriteLine($"GPS: {sumGPS}");
    }

    public static bool IsVerticleDirection((int y, int x) direction)
    {
        return direction switch
        {
            (1, 0) or (-1, 0) => true,
            _ => false
        };
    }
}