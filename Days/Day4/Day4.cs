namespace AdventOfCode2024.Days.Day4;

public class Day4
{
    
    public static void Run()
    {
        var filePath = "Days/Day4/Day4Input.txt";

        var gridList = new List<string>();
        var XMASCounter = 0;
        
        if (File.Exists(filePath))
        {
            gridList = File.ReadAllLines(filePath).ToList();
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }

        var directionalVectors = new List<(int, int)>{(1, 0), (0, 1), (1, -1), (1, 1), (-1, 0), (0, -1), (-1, 1), (-1, -1)};

        for (var y = 0; y < gridList.Count; y++)
        {
            for (var x = 0; x < gridList[y].Length; x++)
            {
                if (gridList[y][x] == 'X')
                {
                    for (var direction = 0; direction < 8; direction++)
                    {
                        if (IsXMAS(gridList, (y, x), directionalVectors[direction]))
                        {
                            XMASCounter++;
                            //Console.WriteLine($"x: {x}, y: {y}, direction: {direction}");
                        }
                    }
                }
            }
        }
        
        Console.WriteLine("XMAS Counter: " + XMASCounter);
    }

    public static bool IsXMAS(List<string> gridList, (int, int) startingPoint, (int, int) directionVector)
    {
        var targetString = "XMAS";
        
        for (int iterations = 0; iterations < 4; iterations++)
        {
            try
            {
                if (gridList[startingPoint.Item1 + directionVector.Item1 * iterations][startingPoint.Item2 + directionVector.Item2 * iterations] !=
                    targetString[iterations])
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        return true;
    }
}