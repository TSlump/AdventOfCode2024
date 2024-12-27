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
        
        var fullNodeDict = new Dictionary<string, HashSet<string>>();

        foreach (var line in input)
        {
            var nodes = line.Split('-');

            if (!fullNodeDict.ContainsKey(nodes[0]))
            {
                fullNodeDict[nodes[0]] = [];
            }

            if (!fullNodeDict.ContainsKey(nodes[1]))
            {
                fullNodeDict[nodes[1]] = [];
            }
            
            fullNodeDict[nodes[0]].Add(nodes[1]);
            fullNodeDict[nodes[1]].Add(nodes[0]);
        }
        
        fullNodeDict = fullNodeDict.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);

        foreach (var kvp in fullNodeDict)
        {
            Console.WriteLine($"{kvp.Key}: {string.Join(",", kvp.Value)}");
        }
        
        Console.WriteLine($"Node Count: {fullNodeDict.Count}");

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
        
        //var largestClique = FindLargestClique(fullNodeDict);
        
        var largestClique = new HashSet<string>();
        BronKerbosch([], [..fullNodeDict.Keys], [], fullNodeDict, largestClique);
        
        largestClique = [..largestClique.OrderBy(x => x)];
        
        Console.WriteLine($"Large Clique Count: {largestClique.Count}");
        Console.WriteLine(string.Join(",", largestClique));
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

    public static HashSet<string> FindLargestClique(Dictionary<string, HashSet<string>> graph)
    {
        var greatestClique = new HashSet<string>();
        
        var potentialCliques = new Queue<HashSet<string>>();

        foreach (var kvp in graph.Where(kvp => kvp.Value.Count > 6))
        {
            potentialCliques.Enqueue([kvp.Key]);
        }

        while (potentialCliques.Count > 0)
        {
            var currentClique = potentialCliques.Dequeue();

            foreach (var kvp in graph)
            {
                if (currentClique.Contains(kvp.Key) || kvp.Value.Count < currentClique.Count)
                {
                    continue;
                }

                if (currentClique.All(node => AreConnected(node, kvp.Key, graph)))
                {
                    var newClique = new HashSet<string>(currentClique) { kvp.Key };

                    potentialCliques.Enqueue(newClique);

                    if (newClique.Count > greatestClique.Count)
                    {
                        greatestClique = newClique;

                        Console.WriteLine($"New largest clique: {string.Join(",", greatestClique)}");
                    }
                }
            }
        }
        
        return greatestClique;
    }

    public static bool AreConnected(string node1, string node2, Dictionary<string, HashSet<string>> graph)
    {
        return graph[node1].Contains(node2);
    }
    
    public static void BronKerbosch(HashSet<string> currentClique, HashSet<string> candidates, HashSet<string> excluded, 
        Dictionary<string, HashSet<string>> graph, HashSet<string> largestClique) 
    {
        if (candidates.Count == 0 && excluded.Count == 0)
        {
            if (currentClique.Count > largestClique.Count)
            {
                largestClique.Clear();
                foreach (var node in currentClique)
                    largestClique.Add(node);
            }
            return;
        }
        
        foreach (var candidate in new HashSet<string>(candidates))
        {
            var newClique = new HashSet<string>(currentClique) { candidate };
            
            var neighbors = graph[candidate];
            
            BronKerbosch(
                newClique,
                [..candidates.Intersect(neighbors)],
                [..excluded.Intersect(neighbors)],
                graph,
                largestClique);
            
            candidates.Remove(candidate);
            excluded.Add(candidate);
        }
    }
    
}