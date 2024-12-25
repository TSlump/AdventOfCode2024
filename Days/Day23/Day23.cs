using Microsoft.VisualBasic;

namespace AdventOfCode2024.Days.Day23;

public class Day23
{
    public static void Run()
    {
        var filePath = "Days/Day23/Day23Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        var nodeDict = new Dictionary<string, List<string>>();

        foreach (var line in input)
        {
            var nodes = line.Split('-').OrderBy(x => x).ToArray();

            if (!nodeDict.ContainsKey(nodes[0]))
            {
                nodeDict[nodes[0]] = [];
            }
            
            nodeDict[nodes[0]].Add(nodes[1]);
        }

        foreach (var kvp in nodeDict)
        {
            Console.WriteLine($"{kvp.Key}: {string.Join(",", kvp.Value)}");
        }

        var tripleCounter = 0;
        
        foreach (var kvp in nodeDict)
        {
            if (kvp.Value.Count > 1)
            {
                var nodePairs = GetNodePairs(kvp.Value);

                foreach (var nodePair in nodePairs)
                {
                    if (nodeDict.TryGetValue(nodePair.Item1, out var value))
                    {
                        if (value.Contains(nodePair.Item2))
                        {
                            if (kvp.Key[0] == 't' || nodePair.Item1[0] == 't' || nodePair.Item2[0] == 't')
                            {
                                tripleCounter++;
                            }
                        }
                    }
                }
            }
        }
        
        Console.WriteLine(tripleCounter);
        
        foreach (var kvp in nodeDict)
        {
            if (kvp.Value.Count > 10)
            {
                var nodeGroups = GetNodeGroups(kvp.Value);
                
                foreach (var nodeGroup in nodeGroups)
                {
                    var isGroup = true;

                    var nodePairs = GetNodePairs(nodeGroup);

                    foreach (var nodePair in nodePairs)
                    {
                        if (nodeDict.TryGetValue(nodePair.Item1, out var value))
                        {
                            if (value.Contains(nodePair.Item2))
                            {
                               // Ignored 
                            }
                            else
                            {
                                isGroup = false;
                            }
                        }
                        else
                        {
                            isGroup = false;
                        }
                    }


                    if (isGroup)
                    {
                        Console.WriteLine($"Group: {kvp.Key},{string.Join(",", nodeGroup)}");
                        return;
                    }
                }
            }
        }
    }

    public static List<(string, string)> GetNodePairs(List<string> nodes)
    {
        var nodePairs = new List<(string, string)>();
        
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                if (string.Compare(nodes[i], nodes[j], StringComparison.Ordinal) < 0)
                {
                    nodePairs.Add((nodes[i], nodes[j]));
                }
                else
                {
                    nodePairs.Add((nodes[j], nodes[i]));
                }
            }
        }
        
        return nodePairs;
    }

    public static List<List<string>> GetNodeGroups(List<string> nodes)
    {
        var nodeGroups = new List<List<string>>();

        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                for (int k = j + 1; k < nodes.Count; k++)
                {
                    for (int l = k + 1; l < nodes.Count; l++)
                    {
                        for (int m = k + 1; m < nodes.Count; m++)
                        {
                            for (int n = k + 1; n < nodes.Count; n++)
                            {
                                for (int o = n + 1; o < nodes.Count; o++)
                                {
                                    for (int p = o + 1; p < nodes.Count; p++)
                                    {
                                        for (int q = k + 1; q < nodes.Count; q++)
                                        {
                                            for (int r = k + 1; r < nodes.Count; r++)
                                            {
                                                for (int t = k + 1; t < nodes.Count; t++)
                                                {
                                                    var group = new List<string>() { nodes[i], nodes[j], nodes[k], nodes[l], nodes[m], nodes[n], nodes[o], nodes[p], nodes[q], nodes[r], nodes[t] };
                                                    group = group.OrderBy(x => x).ToList();
                                                    nodeGroups.Add(group);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        return nodeGroups;
    }

    public static List<string> GenerateCliques(Dictionary<string, List<string>> graph)
    {
        var greatestClique = new List<string>();

        foreach (var kvp in graph)
        {
            
        }
        
        
        
        return greatestClique;
    }
    
}