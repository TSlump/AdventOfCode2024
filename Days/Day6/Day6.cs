namespace AdventOfCode2024.Days.Day6;

using MathNet.Numerics.LinearAlgebra;

public class Day6
{
    public static void RunPartOne()
    {
        var filePath = "Days/Day6/Day6Input.txt";
        
        Guard guard = new Guard();

        string[,] map = GetMapFromFile(filePath, guard);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i,j] == "^")
                {
                    guard.Position = (i, j);
                }
            }
        }

        guard.Map = map;
        
        var uniquePositions = RunGuardSimulation(guard);
        
        Console.WriteLine($"Unique positions: {uniquePositions.Count}");
        
    }

    public static List<(int, int)> RunGuardSimulation(Guard guard)
    {
        var positions = new List<(int,int)>();
        positions.Add(guard.Position);
        
        while (!guard.HasLeft)
        {
            positions.Add(guard.IncrementPosition());
        }
        
        var uniquePositions = positions.Distinct();
        
        return uniquePositions.ToList();
    }

    public static string[,] GetMapFromFile(string filePath, Guard guard)
    {
        string[,] map = new string[,]{};
        
        if (File.Exists(filePath))
        {
            var input = File.ReadAllLines(filePath);
            
            int numRows = input.Length;
            int numCols = input[0].Length;
            
            map = new string[numRows, numCols];
            
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    map[i, j] = input[i][j].ToString();
                }
            }
        }
        else
        {
            Console.WriteLine("File not found");
        }

        return map;
    }

    public static void RunPartTwo()
    {
        var filePath = "Days/Day6/Day6Input.txt";
        
        Guard guard = new Guard();

        string[,] map = GetMapFromFile(filePath, guard);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i,j] == "^")
                {
                    guard.Position = (i, j);
                }
            }
        }
        
        guard.Map = map;
        
        var originalGuardPosition = guard.Position;
        var originalGuardDirection = guard.Direction;
        
        var newObstacleSpots = RunGuardSimulation(guard);
        newObstacleSpots.Remove(originalGuardPosition);

        var correctObstacles = 0;

        foreach (var newObstacleSpot in newObstacleSpots)
        {
            guard.ResetAttributes(originalGuardPosition, originalGuardDirection);
            
            map[newObstacleSpot.Item1, newObstacleSpot.Item2] = "O";
            
            guard.Map = map;

            if (MapCausesLoop(guard))
            {
               correctObstacles += 1; 
            }
            
            map[newObstacleSpot.Item1, newObstacleSpot.Item2] = ".";
        }
        
        Console.WriteLine($"Correct obstacle spots: {correctObstacles}");
    }

    public static bool MapCausesLoop(Guard guard)
    {
        while (!guard.HasLeft)
        {
            guard.IncrementPosition();

            if (guard.IsInALoop)
            {
                return true;
            }
        }

        return false;
    }
}