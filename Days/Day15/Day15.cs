using System.Collections;

namespace AdventOfCode2024.Days.Day15;

public class Day15
{
    public static void RunPartOne()
    {
        var filePath = "Days/Day15/Day15Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input  = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        var grid = new string[input.Length,input[0].Length];

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                grid[y, x] = input[y][x].ToString();
            }
        }

        PrintGrid(grid);

        (int y, int x) start = (grid.GetLength(0) - 2, 1);
        (int y, int x) finish = (1, grid.GetLength(1) - 2);
        
        Console.WriteLine($"Start: {start}, Finish: {finish}, Start: {grid[start.Item1, start.Item2]}, Finish: {grid[finish.Item1, finish.Item2]}");
        
        var (visitedGrid, solutionPaths) = BreadthFirstSearch(grid, start, finish);
        
        Console.WriteLine($"Score: {visitedGrid[finish.y, finish.x].score}");

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (visitedGrid[y, x].wasVisited)
                {
                    Console.Write(visitedGrid[y, x].score + "\t");
                }
                else
                {
                    Console.Write("#\t");
                }
                
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        
        //var tiles = ReverseBreadthFirstSearch(visitedGrid, start, finish);
        
        Console.WriteLine($"Number solutions: {solutionPaths.Count}");
        
        List<(int y, int x)> visited = new List<(int y, int x)>();

        foreach (var solutionPath in solutionPaths)
        {
            foreach (var node in solutionPath.Item1)
            {
                if (solutionPath.score == visitedGrid[finish.y, finish.x].score && !visited.Contains(node))
                {
                    visited.Add(node);
                }
                
            }
            
            Console.WriteLine($"Solution path: {solutionPath.Item1.Count}, Score: {solutionPath.score}");
            Console.WriteLine(string.Join(" ", solutionPath.Item1));
        }
        
        Console.WriteLine($"Number solution nodes: {visited.Count}");
        
        Console.WriteLine($"Tiles: ");
    }

    public static void PrintGrid(string[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                Console.Write(grid[y, x]);
            }
            Console.WriteLine();
        }
    }

    public static ((bool wasVisited, long score)[,], List<(List<(int, int)>, long score)>) BreadthFirstSearch(string[,] grid, (int y, int x) start, (int y, int x) finish)
    {
        Queue<((int y, int x) coords, long score, (int y, int x) direction, List<(int y, int x)> path)> queue = new();
        
        var visited = new (bool wasVisited, long score)[grid.GetLength(0), grid.GetLength(1)];
        
        var solutionPaths = new List<(List<(int, int)>, long score)>();
        
        visited[start.y, start.x] = (true, 0);
        
        queue.Enqueue((start, 0, (0, 1), []));

        while (queue.Count > 0)
        {
            var (currentCoords, currentScore, currentDirection, currentPath) = queue.Dequeue();
            
            //Console.WriteLine($"CurrentCoords: {currentCoords}, CurrentScore: {currentScore}, Direction: {currentDirection}, Path: {string.Join(' ', currentPath)}");

            if (currentCoords == finish)
            {
                var amendedPath = currentPath.ToList();
                amendedPath.Add((currentCoords.y, currentCoords.x));
                
                solutionPaths.Add((amendedPath, currentScore));
                continue;
            }

            if (grid[currentCoords.y + currentDirection.y, currentCoords.x + currentDirection.x] != "#")
            {
                if (!visited[currentCoords.y + currentDirection.y, currentCoords.x + currentDirection.x].wasVisited ||
                    (visited[currentCoords.y + currentDirection.y, currentCoords.x + currentDirection.x].wasVisited &&
                     visited[currentCoords.y + currentDirection.y, currentCoords.x + currentDirection.x].score >=
                     currentScore - 1000))
                {
                    visited[currentCoords.y + currentDirection.y, currentCoords.x + currentDirection.x] =
                        (true, currentScore + 1);

                    var amendedPath = currentPath.ToList();
                    amendedPath.Add((currentCoords.y, currentCoords.x));
                    
                    //Console.WriteLine($"New coords: {(currentCoords.y + currentDirection.y, currentCoords.x + currentDirection.x)}, currentScore: {currentScore + 1}, direction: {currentDirection}, path: {string.Join(' ', amendedPath)}");
                    
                    queue.Enqueue(((currentCoords.y + currentDirection.y, currentCoords.x + currentDirection.x), currentScore + 1, currentDirection, amendedPath));
                }
            }

            (int y, int x)[] newDirections;

            if (currentDirection.y != 0)
            {
                newDirections = [(0, 1), (0, -1)];
            }
            else
            {
                newDirections = [(1, 0), (-1, 0)];
            }

            foreach (var newDirection in newDirections)
            {
                if (grid[currentCoords.y + newDirection.y, currentCoords.x + newDirection.x] != "#")
                {
                    if (!visited[currentCoords.y + newDirection.y, currentCoords.x + newDirection.x].wasVisited ||
                        (visited[currentCoords.y + newDirection.y, currentCoords.x + newDirection.x].wasVisited &&
                         visited[currentCoords.y + newDirection.y, currentCoords.x + newDirection.x].score >=
                         currentScore + 1000))
                    {
                        visited[currentCoords.y + newDirection.y, currentCoords.x + newDirection.x] =
                            (true, currentScore + 1001);
                    
                        var amendedPath = currentPath.ToList();
                        amendedPath.Add((currentCoords.y, currentCoords.x));
                        
                        //Console.WriteLine($"New coords: {(currentCoords.y + newDirection.y, currentCoords.x + newDirection.x)}, currentScore: {currentScore + 1001}, direction: {newDirection}, path: {string.Join(' ', amendedPath)}");
                        
                        queue.Enqueue(((currentCoords.y + newDirection.y, currentCoords.x + newDirection.x), currentScore + 1001, newDirection, amendedPath));
                    }
                }
            }
        }
        
        return (visited, solutionPaths);
    }

    public static long ReverseBreadthFirstSearch((bool wasVisited, long score)[,] prevVisited, (int y, int x) start, (int y, int x) finish)
    {
        long tiles = 0;

        Queue<(int y, int x)> queue = new();
        
        bool[,] newVisited = new bool[prevVisited.GetLength(0), prevVisited.GetLength(1)];
        
        newVisited[finish.y, finish.x] = true;
        
        queue.Enqueue((finish.y, finish.x));

        tiles += 1;

        while (queue.Count > 0)
        {
            var (y, x) = queue.Dequeue();
            
            Console.WriteLine($"Current: {y}, {x}");

            if ((y, x) == start)
            {
                continue;
            }
            
            var directions = new (int y, int x)[]{(-1, 0), (1, 0), (0, -1), (0, 1)};

            foreach (var direction in directions)
            {
                var score = prevVisited[y, x].score;
                
                Console.WriteLine($"Score: {score}, Direction: {direction.y}, {direction.x}");
                
                if (!newVisited[y + direction.y, x + direction.x] && prevVisited[y + direction.y, x + direction.x].wasVisited)
                {
                    Console.WriteLine($"Not visited, viewed score: {prevVisited[y + direction.y, x + direction.x].score}");
                    
                    if (prevVisited[y + direction.y, x + direction.x].score + 1 == score ||
                        prevVisited[y + direction.y, x + direction.x].score + 1001 == score)
                    {
                        newVisited[y + direction.y, x + direction.x] = true;
                        
                        queue.Enqueue((y + direction.y, x + direction.x));

                        tiles += 1;
                    }
                    else
                    {
                        (int y, int x) doubleDirection = (direction.y * 2, direction.x * 2);
                        
                        (int y, int x)[] newDirections;
                        
                        if (direction.y != 0)
                        {
                            newDirections = [(0, 1), (0, -1)];
                        }
                        else
                        {
                            newDirections = [(1, 0), (-1, 0)];
                        }

                        try
                        {
                            if (!newVisited[y + doubleDirection.y, x + doubleDirection.x] &&
                                prevVisited[y + doubleDirection.y, x + doubleDirection.x].wasVisited &&
                                (y, x) != finish)
                            {
                                Console.WriteLine(
                                    $"Not visited, viewed score: {prevVisited[y + doubleDirection.y, x + doubleDirection.x].score}");

                                if (prevVisited[y + doubleDirection.y, x + doubleDirection.x].score + 2 == score ||
                                    prevVisited[y + doubleDirection.y + newDirections[0].y, x + doubleDirection.x + newDirections[0].x].score + 3 == score ||
                                    prevVisited[y + doubleDirection.y + newDirections[1].y, x + doubleDirection.x + newDirections[1].x].score + 3 == score)
                                {
                                    newVisited[y + doubleDirection.y, x + doubleDirection.x] = true;
        
                                    queue.Enqueue((y + doubleDirection.y, x + doubleDirection.x));
        
                                    tiles += 2;
                                }
                            }
                        }
                        catch
                        {
                            // Ignored
                        }
                    }
                }
            }
        }
        
        return tiles;
    }
}