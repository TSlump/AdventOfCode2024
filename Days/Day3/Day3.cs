using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days.Day3;

public class Day3
{
    public static void RunPartOne()
    {
        var filePath = "Days/Day3/Day3Input.txt";
        
        var mulList = new List<string>();
        var pattern = @"^mul\((-?\d+),(-?\d+)\)$"; // chatgpt made the regex pattern

        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var pointer = 0;
                var length = 8;
                
                while (pointer + length <= line.Length)
                {
                    if (Regex.IsMatch(line.Substring(pointer, length), pattern))
                    {
                        mulList.Add(line.Substring(pointer, length));
                        pointer += length;
                        length = 8;
                    }
                    else
                    {
                        if (length < 12)
                        {
                            length += 1;
                        }
                        else
                        {
                            pointer += 1;
                            length = 8;
                        }
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }

        var total = 0;
        
        foreach (var mul in mulList)
        {
            var nums = mul.Replace("mul(", "").Replace(")", "").Split(',');
            
            total += int.Parse(nums[0]) * int.Parse(nums[1]);
        }
        
        Console.WriteLine("total: " + total);
    }
    
    public static void RunPartTwo()
    {
        var filePath = "Days/Day3/Day3Input.txt";
        
        var mulList = new List<string>();
        var pattern = @"^mul\((\d{1,3}),(\d{1,3})\)$";
        var enabled = true;

        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var pointer = 0;
                var length = 8;
                
                while (pointer + length <= line.Length)
                {
                    if (Regex.IsMatch(line.Substring(pointer, length), pattern))
                    {
                        if (enabled)
                        {
                            mulList.Add(line.Substring(pointer, length));
                        }
                        pointer += length;
                        length = 8;
                    }
                    else if (line.Substring(pointer, 4) == "do()")
                    {
                        enabled = true;
                        pointer += 4;
                        length = 8;
                    }
                    else if (line.Substring(pointer, 7) == "don't()")
                    {
                        enabled = false;
                        pointer += 7;
                        length = 8;
                    }
                    else
                    {
                        if (length < 12)
                        {
                            length += 1;
                        }
                        else
                        {
                            pointer += 1;
                            length = 8;
                        }
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }

        var total = 0;
        
        foreach (var mul in mulList)
        {
            var nums = mul.Replace("mul(", "").Replace(")", "").Split(',');
            
            total += int.Parse(nums[0]) * int.Parse(nums[1]);
        }
        
        Console.WriteLine("new total: " + total);
    }
}