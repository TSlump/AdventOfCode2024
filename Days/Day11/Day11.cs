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

    public static void RunPartFour()
    {
        var filePath = "Days/Day11/Day11Input.txt";
        var blinks = 50;
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
        
        var stonesList = new List<double>();
        
        var newStonesList = new List<double>();
        
        var stoneMap = new Dictionary<double, List<double>>();

        foreach (var num in inputNums)
        {
            newStonesList.Add(Convert.ToDouble(num));
        }

        var stoneCount = 0;

        for (var i = 0; i < blinks/5; i++)
        {
            stonesList.AddRange(newStonesList);
            newStonesList = [];

            foreach (var stone in stonesList)
            {
                if (stoneMap.TryGetValue(stone, out var stonesAfterFive))
                {
                    newStonesList.AddRange(stonesAfterFive);
                }
                else
                {
                    var tempStoneList = new List<double>{stone};
                    for (var j = 0; j < 5; j++)
                    {
                        FakeBlink(tempStoneList);
                    }
                    
                    stoneMap.Add(stone, tempStoneList);
                    
                    newStonesList.AddRange(tempStoneList);
                }
            }
        }
       
        //Console.WriteLine($"Stones: {string.Join(" ", newStonesList)}");
        Console.WriteLine($"Stone count: {newStonesList.Count}");
    }

    public static void RunPartFive()
    {
        var filePath = "Days/Day11/Day11Input.txt";
        var blinks = 76;
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

        var stoneTally = new Dictionary<double, double>();
        var nextStoneTally = new Dictionary<double, double>();
        
        foreach (var num in inputNums)
        {
            if (!nextStoneTally.ContainsKey(Convert.ToDouble(num)))
            {
                nextStoneTally[Convert.ToDouble(num)] = 1;
            }
            else
            {
                nextStoneTally[Convert.ToDouble(num)] += 1;
            }
        }
        
        for (var i = 1; i <= blinks; i++)
        {
            
            stoneTally.Clear();
            stoneTally = nextStoneTally.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            nextStoneTally.Clear();

            foreach (var stonePair in stoneTally)
            {
                var newStones = SingleStoneBlink(stonePair.Key);

                if (nextStoneTally.ContainsKey(newStones.Item1))
                {
                    nextStoneTally[newStones.Item1] += stonePair.Value;
                }
                else
                {
                    nextStoneTally.Add(newStones.Item1, stonePair.Value);
                }

                if (newStones.Item2 != null)
                {
                    if (nextStoneTally.ContainsKey(newStones.Item2.Value))
                    {
                        nextStoneTally[newStones.Item2.Value] += stonePair.Value;
                    }
                    else
                    {
                        nextStoneTally.Add(newStones.Item2.Value, stonePair.Value);
                    }
                }
            }
            
        }

        var stoneCount = 0.0;
        foreach (var stonePair in stoneTally)
        {
            stoneCount += stonePair.Value;
        }
        Console.WriteLine($"Stone count: {stoneCount}");
    }
}