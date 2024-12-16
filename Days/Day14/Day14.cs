namespace AdventOfCode2024.Days.Day14;

public class Day14
{
    public static void RunPartOne()
    {
        var filePath = "Days/Day14/Day14Input.txt";
        var gridSize = (101, 103);

        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }

        else
        {
            Console.WriteLine("File not found");
        }
        
        var robots = new List<Robot>();

        foreach (var robotInfo in input)
        {
            var split = robotInfo.Split(" ");

            var coords = split[0].Split("=")[1];
            
            var direction = split[1].Split("=")[1];
            
            robots.Add(new Robot( (Convert.ToInt32(coords.Split(",")[0]), Convert.ToInt32(coords.Split(",")[1])),
                (Convert.ToInt32(direction.Split(",")[0]), Convert.ToInt32(direction.Split(",")[1])),
                gridSize));
        }
        for (int i = 0; i < 10000; i++)
        {
            robots.ForEach(robot => robot.Move());
            
            foreach (var robot in robots){
                //Console.WriteLine($"Robot coords: {robot.Coords.x},{robot.Coords.y}");
            }
            
            //PrintRobots(robots, gridSize);
            
            
            var grid = FillGrid(robots, gridSize);

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1) - 10; col++)
                {
                    var foundLine = true;
                    for (int j = 0; j < 10; j++)
                    {
                        if (grid[row, col + j] != 'X')
                        {
                            foundLine = false;
                            break;
                            
                        }
                    }

                    if (foundLine)
                    {
                        Console.WriteLine($"i: {i}");
                        PrintRobots(robots, gridSize);
                    }
                }
            }
            
            
            
            Console.WriteLine(i);
            
            //Thread.Sleep(1000);

        }
        
        Console.WriteLine($"Centre: {gridSize.Item1 / 2}, {gridSize.Item2 / 2}");
        //var safetyFactor = ComputeSafetyFactor(robots, (gridSize.Item1 / 2, gridSize.Item2 / 2));
        //Console.WriteLine($"Safety factor: {safetyFactor}");
        
        
    }

    public static long ComputeSafetyFactor(List<Robot> robots, (int, int) centre)
    {
        var quadrantCount = new long[] { 0, 0, 0, 0 };

        foreach (var robot in robots)
        {
            if (robot.Coords.Item1 < centre.Item1 && robot.Coords.Item2 < centre.Item2)
            {
                quadrantCount[0]++;
            }

            if (robot.Coords.Item1 > centre.Item1 && robot.Coords.Item2 < centre.Item2)
            {
                quadrantCount[1]++;
            }

            if (robot.Coords.Item1 > centre.Item1 && robot.Coords.Item2 > centre.Item2)
            {
                quadrantCount[2]++;
            }

            if (robot.Coords.Item1 < centre.Item1 && robot.Coords.Item2 > centre.Item2)
            {
                quadrantCount[3]++;
            }
        }
        
        return quadrantCount[0] * quadrantCount[1] * quadrantCount[2] * quadrantCount[3];
    }

    public static void DisplayRobots(List<Robot> robots, (long, long) gridSize)
    {
        var grid = new string[gridSize.Item1, gridSize.Item2];
        
        for (int row = 0; row < gridSize.Item1; row++)
        {
            for (int col = 0; col < gridSize.Item2; col++)
            {
                grid[row, col] = ".";
            }
        }

        foreach (var robot in robots)
        {
            grid[robot.Coords.Item1, robot.Coords.Item2] = "#";
        }

        for (int row = 0; row < gridSize.Item1; row++)
        {
            for (int col = 0; col < gridSize.Item2; col++)
            {
                Console.Write(grid[row, col]);
            }
            Console.WriteLine();
        }
    }

    public static void PrintRobots(List<Robot> robots, (long, long) gridSize)
    {
        var height = gridSize.Item2;
        var width = gridSize.Item1;
        
        // Initialize a 2D grid with the empty character
        char[,] grid = new char[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = ' ';
            }
        }

        // Place robots on the grid
        foreach (var robot in robots)
        {
            int x = robot.Coords.x;
            int y = robot.Coords.y;
            
            grid[y, x] = 'X'; // Flip y to make (0,0) bottom-left
            
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write(grid[i, j]);
            }
            Console.WriteLine();
        }
    }

    public static char[,] FillGrid(List<Robot> robots, (long, long) gridSize)
    {
        var height = gridSize.Item2;
        var width = gridSize.Item1;
        
        // Initialize a 2D grid with the empty character
        char[,] grid = new char[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = ' ';
            }
        }

        // Place robots on the grid
        foreach (var robot in robots)
        {
            int x = robot.Coords.x;
            int y = robot.Coords.y;
            
            grid[y, x] = 'X'; // Flip y to make (0,0) bottom-left
            
        }

        return grid;
    }
}