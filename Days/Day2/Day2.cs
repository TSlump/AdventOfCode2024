namespace AdventOfCode2024.Days.Day2;

public class Day2
{
    public static void Run()
    {
        var filePath = "Days/Day2/Day2Input.txt";

        var safeRecords = 0;

        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var levels = new List<string>(line.Split(" "));

                if (IsSafelyIncreasing(levels) || IsSafelyDecreasing(levels))
                {
                    safeRecords++;
                }
        
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }

        Console.WriteLine($"Safe Records: {safeRecords}");


        // Part 2

        safeRecords = 0;

        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var levels = new List<string>(line.Split(" "));

                if (IsSafelyIncreasingWithDampener(levels) || IsSafelyDecreasingWithDampener(levels))
                {
                    safeRecords++;
                }
        
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }

        Console.WriteLine($"Safe Records with Problem Dampener: {safeRecords}");
    }
    
    public static bool IsSafelyIncreasing(List<string> levels)
    {
        if (levels.Count < 2)
        {
            return false;
        }
    
        var prev = 0;
    
        for (var i = 0; i < levels.Count; i++)
        {
            if (i == 0)
            {
                prev = Convert.ToInt32(levels[i]);
            }
            else
            {
                var current  = Convert.ToInt32(levels[i]);
                if (!(prev < current && current - prev < 4))
                {
                    return false;
                }
            
                prev = current;
            }
        }
    
        return true;
    }

    public static bool IsSafelyDecreasing(List<string> levels)
    {
        if (levels.Count < 2)
        {
            return false;
        }
    
        var prev = 0;
    
        for (var i = 0; i < levels.Count; i++)
        {
            if (i == 0)
            {
                prev = Convert.ToInt32(levels[i]);
            }
            else
            {
                var current  = Convert.ToInt32(levels[i]);
                if (!(prev > current && prev - current < 4))
                {
                    return false;
                }
            
                prev = current;
            }
        }
    
        return true;
    }

    public static bool IsSafelyIncreasingWithDampener(List<string> levels)
    {
        if (IsSafelyIncreasing(levels))
        {
            return true;
        }

        for (var i = 0; i < levels.Count; i++)
        {
            if (IsSafelyIncreasing(levels.Where((item, index) => index != i).ToList()))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsSafelyDecreasingWithDampener(List<string> levels)
    {
        if (IsSafelyDecreasing(levels))
        {
            return true;
        }

        for (var i = 0; i < levels.Count; i++)
        {
            if (IsSafelyDecreasing(levels.Where((item, index) => index != i).ToList()))
            {
                return true;
            }
        }

        return false;
    }
}