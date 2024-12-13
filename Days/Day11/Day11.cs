using System.Collections;

namespace AdventOfCode2024.Days.Day11;

public class Day11
{
    public static void RunTest()
    {
        var blinks = 75;
        var input = "0";
        
        var inputNums = input.Split(" ");
        var stones = new List<double>();

        foreach (var num in inputNums)
        {
            stones.Add(Convert.ToDouble(num));
        }

        for (var i = 1; i <= blinks; i++)
        {
            FakeBlink(stones);
        }
        
        
        Console.WriteLine($"Stone count: {stones.Count}");


    }
    public static void RunPartOne()
    {

        var filePath = "Days/Day11/Day11Input.txt";
        var blinks = 25;
        var input = "";

        if (File.Exists(filePath))
        {
            input = File.ReadAllText(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        var inputNums = input.Split(" ");
        var stones = new List<double>();

        foreach (var num in inputNums)
        {
            stones.Add(Convert.ToDouble(num));
        }

        for (var i = 1; i <= blinks; i++)
        {
            FakeBlink(stones);
        }
        Console.WriteLine($"Stones: {string.Join(" ", stones)}");
        
        Console.WriteLine($"Stone count: {stones.Count}");
        
    }


    public static void Blink(List<double> stones)
    {
        var addedStones = 0;
        var stonesCount = stones.Count;
        
        for (int i = 0; i < stonesCount; i++)
        {
            if (stones[i + addedStones] == 0)
            {
                stones[i + addedStones] = 1;
                
                continue;
            }

            var stoneLength = stones[i + addedStones].ToString().Length;
            
            if (stoneLength % 2 == 0)
            {
                stones.Insert(i + addedStones + 1, Convert.ToDouble(stones[i + addedStones].ToString().Substring(stoneLength / 2, stoneLength / 2)));
                
                stones[i + addedStones] = Convert.ToDouble(stones[i + addedStones].ToString().Substring(0, stoneLength / 2));
                
                addedStones += 1;
                
                continue;
            }

            stones[i + addedStones] *= 2024;
        }
    }
    
    public static void FakeBlink(List<double> stones)
    {
        var stonesCount = stones.Count;
        
        for (int i = 0; i < stonesCount; i++)
        {
            if (stones[i] == 0)
            {
                stones[i] = 1;
                
                continue;
            }

            var stoneLength = stones[i].ToString().Length;
            
            if (stoneLength % 2 == 0)
            {
                stones.Add(Convert.ToDouble(stones[i].ToString().Substring(stoneLength / 2, stoneLength / 2)));
                
                stones[i] = Convert.ToDouble(stones[i].ToString().Substring(0, stoneLength / 2));
                
                continue;
            }

            stones[i] *= 2024;
        }
    }

    public static void RunPartTwo()
    {
        var filePath = "Days/Day11/Day11Input.txt";
        var blinks = 0;
        var input = "";

        if (File.Exists(filePath))
        {
            input = File.ReadAllText(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        var inputNums = input.Split(" ");
        var stonesQueue = new Queue<(int, double)>();

        foreach (var num in inputNums)
        {
            stonesQueue.Enqueue((blinks, Convert.ToDouble(num)));
        }

        var stoneCount = 0;

        while (stonesQueue.Count > 0)
        {
            var stonePair = stonesQueue.Dequeue();
            stoneCount++;
            
            CheekyBlinking(stonePair.Item2, stonePair.Item1, stonesQueue);
        }
        
        Console.WriteLine($"Stone count: {stoneCount}");
    }

    public static void CheekyBlinking(double stoneVal, int blinksLeft, Queue<(int, double)> stonesQueue)
    {
        for (var i = 1; i <= blinksLeft; i++)
        {
            if (stoneVal == 0)
            {
                stoneVal = 1;
                
                continue;
            }

            var stoneLength = stoneVal.ToString().Length;
            
            if (stoneLength % 2 == 0)
            {
                stonesQueue.Enqueue((blinksLeft - i, Convert.ToDouble(stoneVal.ToString().Substring(stoneLength / 2, stoneLength / 2))));
                
                stoneVal = Convert.ToDouble(stoneVal.ToString().Substring(0, stoneLength / 2));
                
                continue;
            }

            stoneVal *= 2024;
        }
        
    }

    public static void RunPartThree()
    {
        var filePath = "Days/Day11/Day11TestInput.txt";
        var blinks = 6;
        var input = "";

        if (File.Exists(filePath))
        {
            input = File.ReadAllText(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        var inputNums = input.Split(" ");
        var stonesQueue = new Queue<(int, double)>();
        var stoneMap = new Dictionary<double, (double, double?)>();

        foreach (var num in inputNums)
        {
            stonesQueue.Enqueue((blinks, Convert.ToDouble(num)));
        }

        var stoneCount = 0;

        while (stonesQueue.Count > 0)
        {
            var stonePair = stonesQueue.Dequeue();
            stoneCount++;
            
            CheekyBlinking2(stonePair.Item2, stonePair.Item1, stonesQueue, stoneMap);
        }
        
        Console.WriteLine($"Stone count: {stoneCount}");
    }

    public static (double, double?) SingleStoneBlink(double stone)
    {
        Console.WriteLine($"Single stone blink: {stone}");
        
        if (stone == 0)
        {
            return (1, null);
        }

        var stoneLength = stone.ToString().Length;
            
        if (stoneLength % 2 == 0)
        {
            return (Convert.ToDouble(stone.ToString().Substring(0, stoneLength / 2)),
                Convert.ToDouble(stone.ToString().Substring(stoneLength / 2, stoneLength / 2)));
        }

        return (stone * 2024, null);
    }

    public static void CheekyBlinking2(double stoneVal, int blinksLeft, Queue<(int, double)> stonesQueue,
        Dictionary<double, (double, double?)> stoneMap)
    {
        for (var i = 1; i <= blinksLeft; i++)
        {
            if (stoneMap.ContainsKey(stoneVal))
            {
                if (stoneMap[stoneVal].Item2 != null)
                {
                    stonesQueue.Enqueue((blinksLeft - i, stoneMap[stoneVal].Item2.Value));
                }
                
                stoneVal = stoneMap[stoneVal].Item1;
            }
            else
            {
                var stonePair = SingleStoneBlink(stoneVal);

                stoneMap[stoneVal] = stonePair;

                if (stonePair.Item2 != null)
                {
                    stonesQueue.Enqueue((blinksLeft - i, stonePair.Item2.Value));
                }
            }
        }
    }
}