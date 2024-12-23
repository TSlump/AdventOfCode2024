namespace AdventOfCode2024.Days.Day21;

public class Day21
{
    public static void Run()
    {
        var filePath = "Days/Day21/Day21TestInput2.txt";
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

            if (yDifference < 0)
            {
                for (int i = 0; i < Math.Abs(yDifference); i++)
                {
                    moveSequence += "^";
                }
            }

            if (xDifference > 0)
            { 
                for (int i = 0; i < xDifference; i++)
                {
                    moveSequence += ">";
                }
            }
            
            if (xDifference < 0)
            { 
                for (int i = 0; i < Math.Abs(xDifference); i++)
                {
                    moveSequence += "<";
                }
            }
            
            if (yDifference > 0)
            {
                for (int i = 0; i < yDifference; i++)
                {
                    moveSequence += "v";
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