namespace AdventOfCode2024.Days.Day8;

public class Day8
{
    public static void Run()
    {
        var filePath = "Days/Day8/Day8Input.txt";
        
        var input = GetInputFromFile(filePath);

        var rows = input.Length;
        var columns = input.First().Length;
        
        var antennas = GetAntennas(input);
        
        var nodes = GenerateNodeLocations(antennas, false);
        
        var superNodes = GenerateNodeLocations(antennas, true, Math.Max(rows, columns));
        
        var uniqueNodes = nodes.Distinct()
            .Where(n => n.Item1 >= 0 && n.Item2 >= 0 && n.Item1 < rows && n.Item2 < columns);
        
        var uniqueSuperNodes = superNodes.Distinct()
            .Where(n => n.Item1 >= 0 && n.Item2 >= 0 && n.Item1 < rows && n.Item2 < columns);
        
        Console.WriteLine($"Unique valid nodes: {uniqueNodes.Count()}");
        
        Console.WriteLine($"Unique super node: {uniqueSuperNodes.Count()}");

    }

    public static string[] GetInputFromFile(string filePath)
    {
        string[] input = [];
        
        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found.");
        }

        return input;
    }

    public static Dictionary<char, List<(int, int)>> GetAntennas(string[] input)
    {
        var antennas = new Dictionary<char, List<(int, int)>>();

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input.Length; j++)
            {
                if (input[i][j] != '.')
                {
                    if (!antennas.ContainsKey(input[i][j]))
                    {
                        antennas.Add(input[i][j], [(i, j)]);
                    }
                    else
                    {
                        antennas[input[i][j]].Add((i, j));
                    }
                }
            }
        }
        
        return antennas;
    }

    public static List<(int, int)> GenerateNodeLocations(Dictionary<char, List<(int, int)>> antennas, bool resonantHarmonics, int iterations = 1)
    {
        var nodes = new List<(int, int)>();

        foreach (var frequency in antennas.Keys)
        {
            foreach (var antenna in antennas[frequency])
            {
                foreach (var otherAntenna in antennas[frequency])
                {
                    if (antenna != otherAntenna)
                    {
                        if (!resonantHarmonics)
                        {
                            nodes.Add( (antenna.Item1 + 2 * (otherAntenna.Item1 - antenna.Item1) , antenna.Item2 + 2 * (otherAntenna.Item2 - antenna.Item2)) );
                        }
                        else
                        {
                            for (int i = 0; i < iterations; i++)
                            {
                                nodes.Add( (antenna.Item1 + i * (otherAntenna.Item1 - antenna.Item1) , antenna.Item2 + i * (otherAntenna.Item2 - antenna.Item2)) );
                            }
                        }
                        
                    }
                }
            }
        }
        
        return nodes;
    }
    
    public static void PrintDictionary(Dictionary<char, List<(int, int)>> dictionary)
    {
        foreach (var kvp in dictionary)
        {
            foreach (var kvp2 in kvp.Value)
            {
                Console.WriteLine($"{kvp.Key}: {kvp2.Item1},{kvp2.Item2}");
            }
        }
    }
}