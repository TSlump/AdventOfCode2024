using System.Runtime.InteropServices.Marshalling;

namespace AdventOfCode2024.Days.Day10;

public class Day10
{
    public static void Run()
    {
        var filePath = "Days/Day10/Day10Input.txt";

        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        int[,] topology = new int[input.Length, input[0].Length];

        for (int i = 0; i < topology.GetLength(0); i++)
        {
            for (int j = 0; j < topology.GetLength(1); j++)
            {
                topology[i, j] = int.Parse(input[i][j].ToString());
            }
        }

        var score = 0;
        
        var rating = 0;

        for (int i = 0; i < topology.GetLength(0); i++)
        {
            for (int j = 0; j < topology.GetLength(1); j++)
            {
                if (topology[i, j] == 0)
                {
                    score += BreadthFirstSearchForScore((i, j), topology);
                    
                    rating += BreadthFirstSearchForRating((i, j), topology);
                }
            }
        }
        
        Console.WriteLine($"Total score: {score}");
        
        Console.WriteLine($"Total rating: {rating}");
    }

    public static int BreadthFirstSearchForScore((int, int) start, int[,] topology)
    {
        var visited = new List<(int, int)>();
        
        var queue = new Queue<(int, int)>();

        var peaks = 0;
        
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();

            if (!visited.Contains(currentPos))
            {
                visited.Add(currentPos);

                if (topology[currentPos.Item1, currentPos.Item2] == 9)
                {
                    peaks++;
                }
                else
                {
                    foreach (var neighbour in GetNeighbours(topology, currentPos))
                    {
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        return peaks;
    }
    
    public static int BreadthFirstSearchForRating((int, int) start, int[,] topology)
    {
        var queue = new Queue<(int, int)>();

        var rating = 0;
        
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            
            if (topology[currentPos.Item1, currentPos.Item2] == 9)
            {
                rating++;
            }
            else
            {
                foreach (var neighbour in GetNeighbours(topology, currentPos))
                {
                    queue.Enqueue(neighbour);
                }
            }
            
        }

        return rating;
    }

    public static List<(int, int)> GetNeighbours(int[,] topology, (int, int) pos)
    {
        var directions = new List<(int, int)> { (1, 0), (0, 1), (-1, 0), (0, -1) };
        
        var neighbours = new List<(int, int)>();

        foreach (var direction in directions)
        {
            try
            {
                if (topology[pos.Item1 + direction.Item1, pos.Item2 + direction.Item2] -
                    topology[pos.Item1, pos.Item2] == 1)
                {
                    neighbours.Add((pos.Item1 + direction.Item1, pos.Item2 + direction.Item2));
                }
            }
            catch (IndexOutOfRangeException)
            {
                continue;
            }
            
        }
        
        return neighbours;
    }
}