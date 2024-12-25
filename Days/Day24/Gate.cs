namespace AdventOfCode2024.Days.Day24;

public class Gate
{
    public string Name { get; set; }

    public (string, string) InputNames { get; set; }
    
    public (int?, int?) InputValues { get; set; } = (null, null);

    public string Operation  { get; set; }

    public int? Output { get; set; } = null;
        
    public Gate(string name, (string, string) inputs, string operation)
    {
        this.Name = name;
        this.InputNames = inputs;
        this.Operation = operation;
    }

    public void ComputeOutput()
    {
        Output = Operation switch
        {
            "OR" => InputValues.Item1 | InputValues.Item2,
            "AND" => InputValues.Item1 & InputValues.Item2,
            "XOR" => InputValues.Item1 ^ InputValues.Item2,
            _ => Output
        };
    }

    public void Reset()
    {
        InputValues = (null, null);
        Output = null;
    }
    
    
}