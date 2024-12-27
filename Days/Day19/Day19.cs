namespace AdventOfCode2024.Days.Day19;

public class Day19
{
    private static int LongestPattern = 0;

    public static void Run()
    {
        var filePath = "Days/Day19/Day19Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        var possibleStripesString = input[0];

        HashSet<string> possibleStripes = new HashSet<string>(
            possibleStripesString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        );

        LongestPattern = possibleStripes.OrderByDescending(s => s.Length).FirstOrDefault()!.Length;

        var totalPossible = 0;

        for (var i = 2; i < input.Length; i++)
        {
            if (IsPossible(possibleStripes, input[i]))
            {
                totalPossible++;
            }
        }

        Console.WriteLine(totalPossible);
        
        long solutions = 0;

        var foundSolutions = new Dictionary<string, long>();
        
        possibleStripes = [..possibleStripes.OrderBy(s => s.Length)];

        foreach (var stripe in possibleStripes)
        {
            foundSolutions[stripe] = NumberOfSolutions(foundSolutions, possibleStripes, stripe);
        }

        foreach (var solution in foundSolutions)
        {
            Console.WriteLine($"{solution.Key}: {solution.Value}");
        }
        
        for (var i = 2; i < input.Length; i++)
        {
            if (IsPossible(possibleStripes, input[i]))
            {
                solutions += NumberOfSolutions(foundSolutions, possibleStripes, input[i]);
            }
        }
        
        Console.WriteLine(solutions);
    }

    public static bool IsPossible(HashSet<string> stripes, string design)
    {
        var print = false;
        
        if (design.Length == 0)
        {
            if (print)
            {
                Console.WriteLine("Valid Combination");
            }
            return true;
        }

        var solFound = false;

        for (var stripeSegmentLength = 1; stripeSegmentLength <= design.Length; stripeSegmentLength++)
        {
            if (stripeSegmentLength > LongestPattern)
            {
                break;
            }
            
            var stripeSegment = design.Substring(0, stripeSegmentLength);

            if (stripes.Contains(stripeSegment))
            {
                if (print)
                {
                    Console.WriteLine($"Stripe: {stripeSegment}");
                }
                
                if (IsPossible(stripes, design.Substring(stripeSegmentLength, design.Length - stripeSegmentLength)))
                {
                    solFound = true;
                    break;
                }
            }
        }

        return solFound;
    }
    
    public static long NumberOfSolutions(Dictionary<string, long> foundSolutions, HashSet<string> possibleStripes, string design)
    {
        if (design.Length == 0)
        {
            return 1;
        }

        if (foundSolutions.TryGetValue(design, out var solutions))
        {
            return solutions;
        }

        long combinations = 0;

        foreach (var stripe in possibleStripes)
        {
            
            if (design.IndexOf(stripe, StringComparison.Ordinal) == 0)
            {
                combinations += NumberOfSolutions(foundSolutions, possibleStripes, design[stripe.Length..]);
            }
        }

        foundSolutions[design] = combinations;
        
        return combinations;
    }
}