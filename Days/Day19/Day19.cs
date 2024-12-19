namespace AdventOfCode2024.Days.Day19;

public class Day19
{
    private static int totalCombinations = 0;

    private static int longestPattern = 0;
    public static void Run()
    {
        var filePath = "Days/Day19/Day19TestInput.txt";
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
        
        longestPattern = possibleStripes.OrderByDescending(s => s.Length).FirstOrDefault()!.Length;

        var totalPossible = 0;

        for (var i = 2; i < input.Length; i++)
        {
            if (isPossible(possibleStripes, input[i]))
            {
                Console.WriteLine(input[i]);
                Console.WriteLine(totalCombinations);
                totalPossible++;
            }
        }
        
        Console.WriteLine(totalPossible);
        
        Console.WriteLine(totalCombinations);
    }

    public static bool isPossible(HashSet<string> stripes, string design)
    {
        var print = false;
        
        if (design.Length == 0)
        {
            if (print)
            {
                Console.WriteLine("Valid Combination");
            }
            totalCombinations += 1;
            return true;
        }

        var solFound = false;

        for (var stripeSegmentLength = 1; stripeSegmentLength <= design.Length; stripeSegmentLength++)
        {
            if (stripeSegmentLength > longestPattern)
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
                
                if (isPossible(stripes, design.Substring(stripeSegmentLength, design.Length - stripeSegmentLength)))
                {
                    solFound = true;
                }
            }
        }

        return solFound;
    }
    
    public static (bool,int) isPossibleWithCounter(HashSet<string> stripes, string design)
    {
        if (design.Length == 0)
        {
            return (true, 0);
        }

        var combinations = 0;
        
        

        for (var stripeSegmentLength = 1; stripeSegmentLength <= design.Length; stripeSegmentLength++)
        {
            var stripeSegment = design.Substring(0, stripeSegmentLength);

            if (stripes.Contains(stripeSegment))
            {
                var futureCombinations = isPossibleWithCounter(stripes,
                    design.Substring(stripeSegmentLength, design.Length - stripeSegmentLength));
                if (futureCombinations.Item1)
                {
                    combinations += futureCombinations.Item2;
                }
            }

            
        }

        if (combinations == 0)
        {
            return (false, 0);
        }
        else
        {
            return (true, combinations);
        }
    }
}