namespace AdventOfCode2024.Days.Day21;

public class Day21
{
    public static void Run()
    {
        var filePath = "Days/Day21/Day21Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        long totalComplexity = 0;

        foreach (var line in input)
        {
            Console.WriteLine(line);
            
            var moveSequence = CodeToNumericKeypad(line);
            
            Console.WriteLine(moveSequence);
            
            var moveSequence2 = SequenceToDirectionalKeypad(moveSequence);
            
            Console.WriteLine(moveSequence2);
            
            var moveSequence3 = SequenceToDirectionalKeypad(moveSequence2);
            
            Console.WriteLine($"{line}: {moveSequence3}");
            
            var complexity = moveSequence3.Length * Convert.ToInt64(line.Split('A')[0]);
            
            Console.WriteLine($"{moveSequence3.Length} * {Convert.ToInt64(line.Split('A')[0])} = {complexity}");
            
            totalComplexity += complexity;
        }
        
        Console.WriteLine($"Total complexity: {totalComplexity}");
        
        Console.WriteLine();
        Console.WriteLine();
    }

    public static void RunPartTwo()
    {
        var filePath = "Days/Day21/Day21Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        long totalComplexity = 0;
        
        var foundSolutionsDict = new Dictionary<(string input, int level), long>();
        
        foreach (var line in input)
        {
            Console.WriteLine(line);
            
            var moveSequence = CodeToNumericKeypad(line);
            
            Console.WriteLine(moveSequence);

            var totalLength = ComputeNextLength(moveSequence, 0, foundSolutionsDict);
            
            Console.WriteLine(totalLength);
            
            var complexity = totalLength * Convert.ToInt64(line.Split('A')[0]);
            
            Console.WriteLine($"{totalLength} * {Convert.ToInt64(line.Split('A')[0])} = {complexity}");
            
            totalComplexity += complexity;
        }
        
        Console.WriteLine($"Total complexity: {totalComplexity}");
        
        Console.WriteLine();
        Console.WriteLine();

        foreach (var (kvp, length) in foundSolutionsDict)
        {
            //Console.WriteLine($"{kvp.input}: {kvp.level}: {length}");
        }
    }

    public static long ComputeNextLength(string input, int level, Dictionary<(string input, int level), long> foundSolutionsDict)
    {
        if (foundSolutionsDict.ContainsKey((input, level)))
        {
            return foundSolutionsDict[(input, level)];
        }

        if (level == 25)
        {
            foundSolutionsDict[(input, level)] = input.Length;
            
            return input.Length;
        }
        
        var sections = input.Split('A');

        for (var i = 0; i < sections.Length - 1; i++)
        {
            sections[i] += "A";
        }
        
        //Console.WriteLine(input);
        
        //Console.WriteLine(string.Join("", sections));
        
        
        //Console.WriteLine(input);
        
        //Console.WriteLine(string.Join(',', sections));

        long total = 0;
        
        var increments = new List<long>();
        
        //Console.WriteLine($"Input: {input}, Level: {level}");
        //Console.WriteLine($"Sections: {string.Join(",", sections)}");

        foreach (var section in sections)
        {
            if (foundSolutionsDict.ContainsKey((section, level)))
            {
                total += foundSolutionsDict[(section, level)];
            }
            else
            {
                var increment = ComputeNextLength(SequenceToDirectionalKeypad(section), level + 1, foundSolutionsDict);
            
                total += increment;
                
                foundSolutionsDict[(section, level)] = increment;
            }
        }
        
        //Console.WriteLine($"Input: {input}, Level: {level}, Total: {total}");
        //Console.WriteLine($"Increments: {string.Join(",", increments)}");
        
        foundSolutionsDict[(input, level)] = total;
        
        return total;
    }

    public static string CodeToNumericKeypad(string code)
    {
        (int y, int x) currentCoords = (3, 2);

        var moveSequence = "";
        
        foreach (char c in code)
        {
            (int y, int x) targetCoords = ConvertKeyToCoords(c);
            
            var xDifference = targetCoords.x - currentCoords.x;
            
            var yDifference = targetCoords.y - currentCoords.y;

            if (currentCoords.y == 3 && targetCoords.x == 0)
            {
                //  Up first
                
                if (yDifference < 0)
                {
                    for (int i = 0; i < Math.Abs(yDifference); i++)
                    {
                        moveSequence += "^";
                    }
                }
            
                if (xDifference < 0)
                { 
                    for (int i = 0; i < Math.Abs(xDifference); i++)
                    {
                        moveSequence += "<";
                    }
                }
            }
            else if (currentCoords.x == 0 && targetCoords.y == 3)
            {
                // Right first
                
                if (xDifference > 0)
                { 
                    for (int i = 0; i < xDifference; i++)
                    {
                        moveSequence += ">";
                    }
                }
                
                if (yDifference > 0)
                {
                    for (int i = 0; i < yDifference; i++)
                    {
                        moveSequence += "v";
                    } 
                }
            }

            else
            {
                
                // Left first
                
                if (xDifference < 0)
                { 
                    for (int i = 0; i < Math.Abs(xDifference); i++)
                    {
                        moveSequence += "<";
                    }
                }
                
                if (yDifference < 0)
                {
                    for (int i = 0; i < Math.Abs(yDifference); i++)
                    {
                        moveSequence += "^";
                    }
                }
                
                if (yDifference > 0)
                {
                    for (int i = 0; i < yDifference; i++)
                    {
                        moveSequence += "v";
                    } 
                }

                if (xDifference > 0)
                { 
                    for (int i = 0; i < xDifference; i++)
                    {
                        moveSequence += ">";
                    }
                }
            }

            moveSequence += "A";
            currentCoords = targetCoords;
        }
        
        return moveSequence;
    }

    public static (int y, int x) ConvertKeyToCoords(char key)
    {
        if (key == 'A')
        {
            return (3, 2);
        }

        if (char.IsDigit(key))
        {
            if (key == '0')
            {
                return (3,1);
            }

            var keyInt  = int.Parse(key.ToString());
            
            return (2 - (keyInt - 1)/ 3, (keyInt - 1) % 3);

        }
        
        return (3, 2);
    }

    public static string SequenceToDirectionalKeypad(string sequence)
    {
        (int y, int x) currentCoords = (0, 2);

        var moveSequence = "";

        foreach (char c in sequence)
        {
            (int y, int x) targetCoords = ConvertDirectionToCoords(c);

            if (currentCoords == (1, 0))
            {
                // Right first
                
                if (targetCoords.x > currentCoords.x)
                {
                    for (int i = 0; i < targetCoords.x - currentCoords.x; i++)
                    {
                        moveSequence += ">";
                    }
                }
                
                if (targetCoords.y < currentCoords.y)
                {
                    moveSequence += "^";
                }
            }
            else if (currentCoords.y == 0 && targetCoords.x == 0)
            {
                // Down first
                
                if (targetCoords.y > currentCoords.y)
                {
                    moveSequence += "v";
                }
                
                if (targetCoords.x < currentCoords.x)
                {
                    for (int i = 0; i < Math.Abs(targetCoords.x - currentCoords.x); i++)
                    {
                        moveSequence += "<";
                    }
                }
            }
            else
            {
                if (targetCoords.x < currentCoords.x)
                {
                    for (int i = 0; i < Math.Abs(targetCoords.x - currentCoords.x); i++)
                    {
                        moveSequence += "<";
                    }
                }
                
                if (targetCoords.y < currentCoords.y)
                {
                    moveSequence += "^";
                }   
                
                if (targetCoords.y > currentCoords.y)
                {
                    moveSequence += "v";
                }
            
                if (targetCoords.x > currentCoords.x)
                {
                    for (int i = 0; i < targetCoords.x - currentCoords.x; i++)
                    {
                        moveSequence += ">";
                    }
                }
            }
            
            moveSequence += "A";
            currentCoords = targetCoords;
        }
        
        return moveSequence;
    }

    public static (int y, int x) ConvertDirectionToCoords(char direction)
    {
        return direction switch
        {
            'A' => (0, 2),
            '^' => (0, 1),
            '<' => (1, 0),
            'v' => (1, 1),
            '>' => (1, 2),
            _ => (0, 2)
        };
    }
}