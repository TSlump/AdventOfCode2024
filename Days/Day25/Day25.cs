namespace AdventOfCode2024.Days.Day25;

public class Day25
{
    public static bool Print = true;
    public static void Run()
    {
        var filePath = "Days/Day25/Day25Input.txt";
        string[] line = [];

        if (File.Exists(filePath))
        {
            line = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        var locks = new List<int[]>();
        var keys = new List<int[]>();

        for (var i = 0; i <= line.Length / 8; i++)
        {
            var lockOrKey = new List<string>();

            for (var j = 0; j < 7; j++)
            {
                lockOrKey.Add(line[i * 8 + j]);
            }

            if (lockOrKey[0] == "#####")
            {
                locks.Add(LockToHeights(lockOrKey));
            }
            else
            {
                keys.Add(KeyToHeights(lockOrKey));
            }
        }

        foreach (var lockHeights in locks)
        {
            Console.WriteLine($"Locked heights: {string.Join(", ", lockHeights)}");
        }

        foreach (var keyHeights in keys)
        {
            Console.WriteLine($"Key heights: {string.Join(", ", keyHeights)}");
        }

        long validKeyLockCombos = 0;

        foreach (var keyHeights in keys)
        {
            foreach (var lockHeights in locks)
            {
                var validCombo = true;
                
                for (var i = 0; i < keyHeights.Length; i++)
                {
                    if (keyHeights[i] + lockHeights[i] > 5)
                    {
                        validCombo = false;
                    }
                }
                if (validCombo)
                {
                    if (Print)
                    {
                        Console.WriteLine($"Lock: [{string.Join(",",lockHeights)}] and Key: {string.Join(",", keyHeights)} fit !");
                    }
                    validKeyLockCombos++;
                }
            }
        }
        
        Console.WriteLine($"Valid key lock combos: {validKeyLockCombos}");
        
    }

    public static int[] KeyToHeights(List<string> key)
    {
        var heights = new int[5];

        for (var layer = 5; layer > 0; layer--)
        {
            for (var i = 0; i < 5; i++)
            {
                if (key[layer][i] == '#')
                {
                    heights[i] += 1;
                }
            }
        }
        
        return heights;
    }

    public static int[] LockToHeights(List<string> key)
    {
        var heights = new int[5];
        
        for (var layer = 1; layer < 6; layer++)
        {
            for (var i = 0; i < 5; i++)
            {
                if (key[layer][i] == '#')
                {
                    heights[i] += 1;
                }
            }
        }
        
        return heights;
    }
}