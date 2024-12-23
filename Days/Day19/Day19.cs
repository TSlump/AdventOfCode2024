namespace AdventOfCode2024.Days.Day19;

public class Day19
{
    private static int totalCombinations = 0;

    private static int longestPattern = 0;

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

        longestPattern = possibleStripes.OrderByDescending(s => s.Length).FirstOrDefault()!.Length;

        var totalPossible = 0;

        for (var i = 2; i < input.Length; i++)
        {
            if (IsPossible(possibleStripes, input[i]))
            {
                Console.WriteLine(input[i]);
                Console.WriteLine(totalCombinations);
                totalPossible++;
            }
        }

        Console.WriteLine(totalPossible);

        Console.WriteLine(totalCombinations);

        var priorityQueue = new PriorityQueue<string, int>();
        
        //var foundSolutions = new Dictionary<string, long>();

        foreach (var stripe in possibleStripes)
        {
            priorityQueue.Enqueue(stripe, stripe.Length);
        }

        // while (priorityQueue.Count > 0)
        // {
        //     var stripe = priorityQueue.Dequeue();
        //
        //     totalCombinations = 0;
        //     
        //     IsPossibleWithCount(possibleStripes, stripe);
        //     
        //     foundSolutions[stripe] = totalCombinations;
        // }
        //
        // foreach (var kvp in foundSolutions)
        // {
        //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        // }
        
        var foundSolutions = new Dictionary<string, HashSet<string[]>>
        {
            { "r", new HashSet<string[]> { new string[] { "r" } } },
            { "wr", new HashSet<string[]> { new string[] { "wr" } } },
            { "b", new HashSet<string[]> { new string[] { "b" } } },
            { "g", new HashSet<string[]> { new string[] { "g" } } },
            { "bwu", new HashSet<string[]> { new string[] { "bwu" } } },
            { "rb", new HashSet<string[]> { new string[] { "rb" }, new string[] { "r", "b" } } },
            { "gb", new HashSet<string[]> { new string[] { "gb" }, new string[] { "g", "b" } } },
            { "br", new HashSet<string[]> { new string[] { "br" }, new string[] { "b", "r" } } }
        };

        foundSolutions = [];

        priorityQueue = new PriorityQueue<string, int>();

        foreach (var stripe in possibleStripes)
        {
            priorityQueue.Enqueue(stripe, stripe.Length);
        }

        var print = false;
        
        while (priorityQueue.Count > 0)
        {
            var stripe = priorityQueue.Dequeue();
            
            NumberOfSolutionsAsList(foundSolutions, stripe);
            
            foundSolutions[stripe].Add(new string[] { stripe });
            
            if (print)
            {
                foreach (var solution in foundSolutions[stripe])
                {
                    Console.WriteLine("[" + string.Join(", ", solution) + "]");
                }
            }
            
        }

        long total = 0;
        
        for (var i = 2; i < input.Length; i++)
        {
            var num = NumberOfSolutionsAsList(foundSolutions, input[i]);

            if (print)
            {
              foreach (var solution in foundSolutions[input[i]])
              {
                  Console.WriteLine("[" + string.Join(", ", solution) + "]");
              }  
            }
            total += num.Count;
            
            Console.WriteLine($"Design: {input[i]}, Num: {num.Count}");
        }
        
        Console.WriteLine(total);
        
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
            //totalCombinations += 1;
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
                
                if (IsPossible(stripes, design.Substring(stripeSegmentLength, design.Length - stripeSegmentLength)))
                {
                    solFound = true;
                    break;
                }
            }
        }

        return solFound;
    }
    
    public static bool IsPossibleWithCount(HashSet<string> stripes, string design)
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
                    Console.WriteLine($"design length: {design.Length},     segment length: {stripeSegmentLength}");
                }
                
                if (IsPossibleWithCount(stripes, design.Substring(stripeSegmentLength, design.Length - stripeSegmentLength)))
                {
                    solFound = true;
                }
            }
        }

        return solFound;
    }
    
    public static long NumberOfSolutions(Dictionary<string, long> foundSolutions, string design)
    {
        var print = false;
        
        if (design.Length == 0)
        {
            Console.WriteLine("Empty");
            return 0;
        }

        if (foundSolutions.ContainsKey(design))
        {
            Console.WriteLine($"Design: {design}, found in solutions: {foundSolutions[design]}");
            return foundSolutions[design];
        }

        long combinations = 0;
        
        for (var stripeSegmentLength = 1; stripeSegmentLength < design.Length; stripeSegmentLength++)
        {
            Console.WriteLine($"Design: {design}");
            var leftSegmentSol = NumberOfSolutions(foundSolutions, design[..stripeSegmentLength]);
            var rightSegmentSol = NumberOfSolutions(foundSolutions, design[stripeSegmentLength..]);
            
            combinations += leftSegmentSol * rightSegmentSol;

            if (print)
            {
                Console.WriteLine($"Design: {design}");
                Console.WriteLine($"Left: {design[..stripeSegmentLength]}, sol: {leftSegmentSol}");
                Console.WriteLine($"Right: {design[stripeSegmentLength..]}, sol: {rightSegmentSol}");
            }
        }

        foundSolutions[design] = combinations;

        if (print)
        {
            Console.WriteLine($"Returned:   Design: {design}, combinations: {combinations}");
        }
        return combinations;
    }
    
    public static HashSet<string[]> NumberOfSolutionsAsList(Dictionary<string, HashSet<string[]>> foundSolutions, string design)
    {
        var print = false;
        
        if (design.Length == 0)
        {
            //Console.WriteLine("Empty");
            return [];
        }

        if (foundSolutions.ContainsKey(design))
        {
            //Console.WriteLine($"Design: {design}, found in solutions: {foundSolutions[design]}");
            return foundSolutions[design];
        }

        var combinations = new HashSet<string[]>(new StringArrayComparer());
        
        for (var stripeSegmentLength = 1; stripeSegmentLength < design.Length; stripeSegmentLength++)
        {
            //Console.WriteLine($"Design: {design}");
            var leftSegmentSol = NumberOfSolutionsAsList(foundSolutions, design[..stripeSegmentLength]);
            var rightSegmentSol = NumberOfSolutionsAsList(foundSolutions, design[stripeSegmentLength..]);
            
            foreach (var left in leftSegmentSol)
            {
                foreach (var right in rightSegmentSol)
                {
                    // Concatenate the arrays and add to the result
                    var combinedSolution = left.Concat(right).ToArray();
                    combinations.Add(combinedSolution);
                }
            }

            if (print)
            {
                Console.WriteLine($"Design: {design}");
                Console.WriteLine($"Left: {design[..stripeSegmentLength]}, sol: {leftSegmentSol.Count}");
                Console.WriteLine($"Right: {design[stripeSegmentLength..]}, sol: {rightSegmentSol.Count}");
            }
        }

        foundSolutions[design] = combinations;

        if (print)
        {
            Console.WriteLine($"Returned:   Design: {design}, combinations: {combinations}");
        }
        return combinations;
    }
}


// Chat wrote the following StringArrayComparer, didn't know how to get around the HashSets not actually working on arrays or lists.
public class StringArrayComparer : IEqualityComparer<string[]>
{
    public bool Equals(string[] x, string[] y)
    {
        if (x.Length != y.Length) return false;
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i]) return false;
        }
        return true;
    }

    public int GetHashCode(string[] obj)
    {
        int hash = 17;
        foreach (var item in obj)
        {
            hash = hash * 31 + (item?.GetHashCode() ?? 0);
        }
        return hash;
    }
}