using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days.Day24;

public class Day24
{
    public static bool Print = true;
    public static void Run()
    {
        var filePath = "Days/Day24/Day24Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        var gapIndex = 0;
        
        for (var i = 0; i < input.Length; i++)
        {
            if (string.IsNullOrEmpty(input[i]))
            {
                gapIndex = i;
            }
        }
        
        var initialWires = input.Take(gapIndex).ToList();
        
        var gates = input.Skip(gapIndex).ToList();

        if (Print)
        {
            Console.WriteLine($"Initial wires: \n{string.Join("\n", initialWires)}");
                    
            Console.WriteLine($"Gates: {string.Join("\n", gates)}");
        }
        
        var gatesDictionary = new Dictionary<string, Gate>();

        var wireLinks = new Dictionary<string, List<string>>();

        foreach (var gate in gates)
        {
            // Chat wrote regex for me
            var pattern = @"(\w+)\s+(\w+)\s+(\w+)\s+->\s+(\w+)";
            
            var match = Regex.Match(gate, pattern);
            
            if (!match.Success) continue;
            
            var input1 = match.Groups[1].Value;
            var operation = match.Groups[2].Value;
            var input2 = match.Groups[3].Value;
            var output = match.Groups[4].Value;
            
            var gateObject = new Gate(output, (input1, input2), operation);
            
            gatesDictionary.Add(output, gateObject);

            if (wireLinks.ContainsKey(input1))
            {
                wireLinks[input1].Add(output);
            }
            else
            {
                wireLinks.Add(input1, [output]);
            }
            
            if (wireLinks.ContainsKey(input2))
            {
                wireLinks[input2].Add(output);
            }
            else
            {
                wireLinks.Add(input2, [output]);
            }
        }

        if (Print)
        {
          foreach (var gate in gatesDictionary)
          {
              Console.WriteLine($"{gate.Key}: {gate.Value.InputNames.Item1} {gate.Value.Operation} {gate.Value.InputNames.Item2}");
          }
  
          foreach (var gate in wireLinks)
          {
              Console.WriteLine($"{gate.Key}: {string.Join(" ", gate.Value)}");
          }  
        }

        // var GatesToSwap = new List<(string, string)> { ("z21", "shh" ), ("vgs", "dtk"), ("z33", "dqr") };
        var GatesToSwap = new List<(string, string)> { ("z21", "shh" ), ("z33", "dqr"), ("z39", "pfw"), ("vgs", "dtk") };
        
        Console.WriteLine();
        Console.WriteLine($"x39: {string.Join(" ", wireLinks["x39"])}");
        Console.WriteLine($"y39: {string.Join(" ", wireLinks["y39"])}");
        Console.WriteLine($"gqn: {string.Join(" ", wireLinks["gqn"])}");
        Console.WriteLine($"sjq: {string.Join(" ", wireLinks["sjq"])}");
        
        wireLinks["mtw"] = ["dqr"];
        wireLinks["fdv"] = ["dqr"];
        wireLinks["vkp"] = ["fdv", "z33"];
        wireLinks["jqp"] = ["fdv", "z33"];

        wireLinks["x39"] = ["gqn", "pfw"];
        wireLinks["y39"] = ["gqn", "pfw"];
        wireLinks["gqn"] = ["z39", "kdd"];
        wireLinks["sjq"] = ["z39", "kdd"];
        
        Console.WriteLine();
        Console.WriteLine($"mtw: {string.Join(" ", wireLinks["mtw"])}");
        Console.WriteLine($"fdv: {string.Join(" ", wireLinks["fdv"])}");
        Console.WriteLine($"vkp: {string.Join(" ", wireLinks["vkp"])}");
        Console.WriteLine($"jqp: {string.Join(" ", wireLinks["jqp"])}");
        Console.WriteLine("");

        foreach (var gateToSwap in GatesToSwap)
        {
            SwapGates(gateToSwap.Item1, gateToSwap.Item2, gatesDictionary, wireLinks);
        }
        
        foreach (var wire in initialWires)
        {
            var wireName = wire.Split(":")[0];
            
            var wireValue = Convert.ToInt32(wire.Split(":")[1].Replace(" ", ""));
            
            UpdateWires(wireName, wireValue, gatesDictionary, wireLinks);
        }

        long xTotal = 0;
        long yTotal = 0;        
        long zTotal = 0;

        foreach (var wire in initialWires)
        {
            var wireName = wire.Split(":")[0];
            
            var wireValue = Convert.ToInt32(wire.Split(":")[1].Replace(" ", ""));

            if (wireName[0] == 'x')
            {
                xTotal += wireValue * (long)Math.Pow(2, int.Parse(wireName[1..]));
            }

            if (wireName[0] == 'y')
            {
                yTotal += wireValue * (long)Math.Pow(2, int.Parse(wireName[1..]));
            }
        }

        foreach (var gate in gatesDictionary)
        {
            if (gate.Key[0] == 'z')
            {
                Console.WriteLine($"{gate.Key}: {gate.Value.Output}");

                if (gate.Value.Output != null)
                {
                    zTotal += gate.Value.Output!.Value * (long)Math.Pow(2, int.Parse(gate.Key[1..]));
                }
            }
        }
        
        Console.WriteLine($"Total Z wires: {zTotal}");
        Console.WriteLine($"x: {xTotal}, y: {yTotal}, z: {zTotal}");
        
        Console.WriteLine($"     X:  {Convert.ToString(xTotal, 2)}");
        Console.WriteLine($"     Y:  {Convert.ToString(yTotal, 2)}");
        Console.WriteLine($"Fake Z: {Convert.ToString(zTotal, 2)}");

        var realTotal = xTotal + yTotal;
        
        Console.WriteLine($"Real Z: {Convert.ToString(realTotal, 2)}");

        var difference = zTotal ^ realTotal;
        
        Console.WriteLine($"Diff Z:      {Convert.ToString(difference, 2)}");
        
        Console.WriteLine($"Location of Diff: {Convert.ToString(difference, 2).Length - Convert.ToString(difference, 2).LastIndexOf("1", StringComparison.Ordinal)}");
        
        Console.WriteLine();
        

        Inspect("z04", gatesDictionary, 7);
        Console.WriteLine();
        
        Inspect("z40", gatesDictionary, 7);
        Console.WriteLine();
        
        Inspect("z39", gatesDictionary, 7);
        Console.WriteLine();
        
        Inspect("z38", gatesDictionary, 7);
        Console.WriteLine();

        foreach (var gate in gatesDictionary)
        {
            if (gate.Key[0] == 'z')
            {
                Console.WriteLine($"{gate.Key}: {gate.Value.Operation}");
            }
        }

        var SwappedGates = new List<string>();

        foreach (var gate in GatesToSwap)
        {
            SwappedGates.Add(gate.Item1);
            SwappedGates.Add(gate.Item2);
        }
        
        SwappedGates.Sort();
        
        Console.WriteLine($"Gates To Swap: {string.Join(",", SwappedGates)}");

    }

    public static void UpdateWires(string wireName, int wireValue, Dictionary<string, Gate> gates,
        Dictionary<string, List<string>> wireLinks)
    {
        if (!wireLinks.TryGetValue(wireName, out var links))
        {
            return;
        }
        
        foreach (var gate in links)
        {
            if (gates[gate].InputNames.Item1 == wireName)
            {
                gates[gate].InputValues = gates[gate].InputValues with { Item1 = wireValue };
            }
            
            if (gates[gate].InputNames.Item2 == wireName)
            {
                gates[gate].InputValues = gates[gate].InputValues with { Item2 = wireValue };
            }

            if (gates[gate].InputValues.Item1.HasValue && gates[gate].InputValues.Item2.HasValue)
            {
                gates[gate].ComputeOutput();
                
                UpdateWires(gate, gates[gate].Output!.Value, gates, wireLinks);
            }
        }
    }

    public static void Inspect(string gateName, Dictionary<string, Gate> gates, int depth)
    {
        if (depth == 0 || gateName[0] == 'x' || gateName[0] == 'y')
        {
            return;
        }
        
        var gate = gates[gateName];
        
        Console.WriteLine($"{gate.Name} : {gate.InputNames.Item1} {gate.Operation} {gate.InputNames.Item2}");
        Console.WriteLine($"{gate.Output!.Value} : {gate.InputValues.Item1} {gate.Operation} {gate.InputValues.Item2}");
        
        Inspect(gate.InputNames.Item1, gates, depth - 1);
        
        Inspect(gate.InputNames.Item2, gates, depth - 1);
    }

    public static void SwapGates(string gate1, string gate2, Dictionary<string, Gate> gates, Dictionary<string, List<string>> wireLinks)
    {
        var gate1Inputs = gates[gate1].InputNames;
        var gate1Operation = gates[gate1].Operation;
        
        gates[gate1].InputNames = gates[gate2].InputNames;
        gates[gate1].Operation = gates[gate2].Operation;
        
        gates[gate2].InputNames = gate1Inputs;
        gates[gate2].Operation = gate1Operation;
        
        //wireLinks[gates[gate2].InputNames.Item1][wireLinks[gates[gate2].InputNames.Item1].IndexOf]
    }
}