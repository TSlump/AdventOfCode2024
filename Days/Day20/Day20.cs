namespace AdventOfCode2024.Days.Day20;

public class Day20
{
    public static void Run()
    {
        var filePath = "Days/Day20/Day20Input.txt";
        string[] input = [];
        var print = false;

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        var grid = new string[input.Length, input[0].Length];
        var trackLength = 0;
        (int y, int x) startPosition = (0, 0);
        (int y, int x) endPosition = (0, 0);

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[0].Length; col++)
            {
                grid[row, col] = input[row][col].ToString();

                if (grid[row, col] == ".")
                {
                    trackLength++;
                }
                else if (grid[row, col] == "S")
                {
                    startPosition = (row, col);
                }
                else if (grid[row, col] == "E")
                {
                    endPosition = (row, col);
                    trackLength++;
                }
            }
        }

        if (print)
        {
            Console.WriteLine($"track length: {trackLength}");
            Console.WriteLine($"start position: {startPosition.y}, {startPosition.x}");
            Console.WriteLine($"end position: {endPosition.y}, {endPosition.x}");
        }

        var trackFinishLength = new int?[input.Length, input[0].Length];
        
        var currentCoords = startPosition;
        var currentLength = 0;

        while (currentCoords != endPosition)
        {
            trackFinishLength[currentCoords.y, currentCoords.x] = trackLength - currentLength;
            
            var directions = new (int y, int x)[]{(1, 0), (0, 1), (0, -1), (-1, 0)};

            foreach (var direction in directions)
            {
                if (grid[currentCoords.y + direction.y, currentCoords.x + direction.x] != "#" &&
                    !trackFinishLength[currentCoords.y + direction.y, currentCoords.x + direction.x].HasValue)
                {
                    currentCoords.y += direction.y;
                    currentCoords.x += direction.x;

                    break;
                }
            }

            currentLength++;
        }
        
        trackFinishLength[endPosition.y, endPosition.x] = 0;

        if (print)
        {
            Console.WriteLine($"track length: {trackFinishLength[1, 1]}");
        }

        var visitedGrid = new bool[input[0].Length, input[0].Length];
        
        Console.WriteLine($"Start position: {startPosition.y}, {startPosition.x}");
        currentCoords.y = startPosition.y;
        currentCoords.x = startPosition.x;
        
        visitedGrid[currentCoords.y, currentCoords.x] = true;
        
        Console.WriteLine($"Current position: {currentCoords.y}, {currentCoords.x}");
        
        var timeSavedTally = new int[trackLength];
        
        
        
        while (currentCoords != endPosition)
        {
            var directions = new (int y, int x)[]{(1, 0), (0, 1), (0, -1), (-1, 0)};

            if (print)
            {
                Console.WriteLine($"Current position: {currentCoords.y}, {currentCoords.x}");
            }

            foreach (var direction in directions)
            {
                try
                {
                    if (grid[currentCoords.y + 2 * direction.y, currentCoords.x + 2 * direction.x] != "#")
                    {
                        var timeSaved = trackFinishLength[currentCoords.y, currentCoords.x]!.Value -
                                        trackFinishLength[currentCoords.y + 2 * direction.y,
                                            currentCoords.x + 2 * direction.x]!.Value - 2;

                        if (timeSaved >= 0)
                        {
                            timeSavedTally[timeSaved] += 1;
                        }
                    }
                }
                catch
                {
                    //Ignored
                }
                
            }
            
            foreach (var direction in directions)
            {
                if (print)
                {
                    Console.WriteLine($"Current direction: {direction.y}, {direction.x}, looking at {currentCoords.y + direction.y}, {currentCoords.x + direction.x} which is {grid[currentCoords.y + direction.y, currentCoords.x + direction.x]}");
                    
                    Console.WriteLine($"Has Value: {trackFinishLength[currentCoords.y + direction.y, currentCoords.x + direction.x].HasValue}");
                    
                    Console.WriteLine($"Track length of looked space: {trackFinishLength[currentCoords.y + direction.y, currentCoords.x + direction.x]}");
                    
                    //Console.WriteLine($"Prev coords: {prevCoords.y}, {prevCoords.x}");
                    
                    
                    System.Threading.Thread.Sleep(100);
                }
                
                if (trackFinishLength[currentCoords.y + direction.y, currentCoords.x + direction.x].HasValue &&
                    !visitedGrid[currentCoords.y + direction.y, currentCoords.x + direction.x])
                {
                    visitedGrid[currentCoords.y + direction.y, currentCoords.x + direction.x] = true;
                    
                    currentCoords.y += direction.y;
                    currentCoords.x += direction.x;
                    
                    if (print)
                    {
                        Console.WriteLine($"Moved to: {currentCoords.y}, {currentCoords.x}");
                    }

                    break;
                }
            }
        }

        
        Console.WriteLine("Tally Results:");
        for (int i = 0; i < timeSavedTally.Length; i++)
        {
            if (timeSavedTally[i] > 0)
            {
                Console.WriteLine($"Index {i}: {timeSavedTally[i]}");
            }
        }

        var runningTotalAbove100 = 0;
        for (int i = 100; i < timeSavedTally.Length; i++)
        {
            runningTotalAbove100 += timeSavedTally[i];
        }
        Console.WriteLine($"Running total above 100: {runningTotalAbove100}");
        
        
        visitedGrid = new bool[input[0].Length, input[0].Length];
        
        Console.WriteLine($"Start position: {startPosition.y}, {startPosition.x}");
        currentCoords.y = startPosition.y;
        currentCoords.x = startPosition.x;
        
        visitedGrid[currentCoords.y, currentCoords.x] = true;
        
        Console.WriteLine($"Current position: {currentCoords.y}, {currentCoords.x}");
        
        timeSavedTally = new int[trackLength];
        
        
        
        while (currentCoords != endPosition)
        {
            var directions = new (int y, int x)[]{(1, 0), (0, 1), (-1, 0), (0, -1)};

            for (var i = 0; i < directions.Length; i++)
            {
                for (int directionOneCount = 1; directionOneCount <= 20; directionOneCount++)
                {
                    for (int directionTwoCount = 0; directionTwoCount <= 20 - directionOneCount; directionTwoCount++)
                    {
                        try
                        {
                            if (grid[currentCoords.y + directionOneCount * directions[i].y + directionTwoCount * directions[(i+1)%4].y, currentCoords.x + directionOneCount * directions[i].x + directionTwoCount * directions[(i+1)%4].x] != "#")
                            {
                                var timeSaved = trackFinishLength[currentCoords.y, currentCoords.x]!.Value -
                                                trackFinishLength[currentCoords.y + directionOneCount * directions[i].y + directionTwoCount * directions[(i+1)%4].y, currentCoords.x + directionOneCount * directions[i].x + directionTwoCount * directions[(i+1)%4].x]!.Value - (directionOneCount + directionTwoCount);
        
                                if (timeSaved >= 0)
                                {
                                    timeSavedTally[timeSaved] += 1;
                                }
                            }
                        }
                        catch
                        {
                            //Ignored
                        }
                    }
                }
                
                
                
                
            }
            
            foreach (var direction in directions)
            {
                
                if (trackFinishLength[currentCoords.y + direction.y, currentCoords.x + direction.x].HasValue &&
                    !visitedGrid[currentCoords.y + direction.y, currentCoords.x + direction.x])
                {
                    visitedGrid[currentCoords.y + direction.y, currentCoords.x + direction.x] = true;
                    
                    currentCoords.y += direction.y;
                    currentCoords.x += direction.x;

                    break;
                }
            }
        }
        
        Console.WriteLine("Tally Results:");
        for (int i = 0; i < timeSavedTally.Length; i++)
        {
            if (timeSavedTally[i] > 0)
            {
                Console.WriteLine($"Index {i}: {timeSavedTally[i]}");
            }
        }
        
        runningTotalAbove100 = 0;
        for (int i = 100; i < timeSavedTally.Length; i++)
        {
            runningTotalAbove100 += timeSavedTally[i];
        }
        Console.WriteLine($"Running total above 100: {runningTotalAbove100}");
    }
}