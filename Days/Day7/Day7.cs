using MathNet.Numerics;

namespace AdventOfCode2024.Days.Day7;

public class Day7
{
    public static void Run()
    {
        var filePath = "Days/Day7/Day7Input.txt";

        var input = GetInputFromFile(filePath);

        var totalCalibration = 0.0;
        var totalCalibrationWithConcatOperation = 0.0;

        foreach (var equations in input)
        {
            if (GivesCorrectInput(equations.Item1, equations.Item2, false))
            {
                totalCalibration += equations.Item1;
            }

            if (GivesCorrectInput(equations.Item1, equations.Item2, true))
            {
                totalCalibrationWithConcatOperation += equations.Item1;
            }
        }
        
        Console.WriteLine($"Total Calibration: {totalCalibration}");
        
        Console.WriteLine($"Total Calibration with Concat: {totalCalibrationWithConcatOperation}");

    }

    public static List<(double, List<double>)> GetInputFromFile(string filePame)
    {
        var inputReturn = new List<(double, List<double>)>();
        
        if (File.Exists(filePame))
        {
            var input = File.ReadAllLines(filePame);

            foreach (var line in input)
            {
                var separated = line.Split(':');
                var intOptions = separated[1].Trim().Split(' ').Select(double.Parse).ToList();
                
                inputReturn.Add((Convert.ToDouble(separated[0]), intOptions));
            }
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        return inputReturn;
    }

    public static bool GivesCorrectInput(double answer, List<double> remainingInputs, bool concatOperation)
    {
        if (remainingInputs.Count == 1)
        {
            if (Math.Abs(answer - remainingInputs[0]) < 0.1)
            {
                return true;
            }
            return false;
        }

        if (remainingInputs.Last() > answer)
        {
            return false;
        }
        
        if (answer % remainingInputs.Last() == 0)
        {
            if (GivesCorrectInput(answer / remainingInputs.Last(), remainingInputs.GetRange(0, remainingInputs.Count - 1).ToList(), concatOperation))
            {
                return true;
            }
        }
        
        if (GivesCorrectInput(answer - remainingInputs.Last(), remainingInputs.GetRange(0, remainingInputs.Count - 1).ToList(), concatOperation))
        {
            return true;
        }

        if (concatOperation && Math.Abs(answer - remainingInputs.Last()) > 0.1)
        {
            var ansString = answer.ToString();
            var inputString = remainingInputs.Last().ToString();
            
            if (ansString.EndsWith(inputString))
            {
                var newAnsString = ansString.Substring(0, ansString.Length - inputString.Length);
                
                if (GivesCorrectInput(Convert.ToDouble(newAnsString),
                        remainingInputs.GetRange(0, remainingInputs.Count - 1).ToList(), concatOperation))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    public static double CombineTwoInputs(List<double> remainingInputs)
    {
        return Math.Pow(10,Math.Ceiling(Math.Log10(remainingInputs[1]))) * remainingInputs[0] + remainingInputs[1];
    }
}