using AdventOfCode2024.Days.Day1;
using AdventOfCode2024.Days.Day10;
using AdventOfCode2024.Days.Day11;
using AdventOfCode2024.Days.Day12;
using AdventOfCode2024.Days.Day13;
using AdventOfCode2024.Days.Day14;
using AdventOfCode2024.Days.Day15;
using AdventOfCode2024.Days.Day17;
using AdventOfCode2024.Days.Day18;
using AdventOfCode2024.Days.Day2;
using AdventOfCode2024.Days.Day3;
using AdventOfCode2024.Days.Day4;
using AdventOfCode2024.Days.Day5;
using AdventOfCode2024.Days.Day6;
using AdventOfCode2024.Days.Day7;
using AdventOfCode2024.Days.Day8;
using AdventOfCode2024.Days.Day9;
using AdventOfCode24.Days.Day15;

Console.WriteLine("Advent of Code-2024 \n");

var day = 17;

switch (day)
{
    case 1:
        Console.WriteLine("Day 1");
        Day1.Run();
        break;
        
    case 2:
        Console.WriteLine("\nDay 2\n");
        Day2.Run();
        break;
        
    case 3:
        Console.WriteLine("\nDay 3\n");
        Day3.RunPartOne();
        Day3.RunPartTwo();
        break;
    
    case 4:
        Console.WriteLine("\nDay 4\n");
        Day4.RunPartOne();
        Day4.RunPartTwo();
        break;
    
    case 5:
        Console.WriteLine("\nDay 5\n");
        Day5.Run();
        break;
    
    case 6:
        Console.WriteLine("\nDay 6\n");
        Day6.RunPartOne();
        Day6.RunPartTwo();
        break;
    
    case 7:
        Console.WriteLine("\nDay 7\n");
        Day7.Run();
        break;
    
    case 8:
        Console.WriteLine("\nDay 8\n");
        Day8.Run();
        break;
    
    case 9:
        Console.WriteLine("\nDay 9\n");
        Day9.Run();
        break;
    
    case 10:
        Console.WriteLine("\nDay 10\n");
        Day10.Run();
        break;
    
    case 11:
        Console.WriteLine("\nDay 11\n");
        //Day11.RunPartThree();
        //Day11.RunPartOne();
        //Day11.RunPartTwo();
        Day11.RunPartFive();
        break;
    
    case 12:
        Console.WriteLine("\nDay 12\n");
        Day12.Run();
        break;
    
    case 13:
        Console.WriteLine("\nDay 13\n");
        //Day13.RunPartOne();
        //Day13.RunPartTwo();
        Day13.LineIntersection();
        break;
    
    case 14:
        Console.WriteLine("\nDay 14\n");
        Day14.RunPartOne();
        break;
    
    case 15:
        Console.WriteLine("\nDay 15\n");
        Day15.RunPartOne();
        break;
    
    case 16:
        Console.WriteLine("\nDay 16\n");
        Day16.RunPartOne();
        break;
    
    case 17:
        Console.WriteLine("\nDay 17\n");
        Day17.Run();
        //Day17.RunPartTwo();
        //Day17.TestInputs();
        //Day17.RunPartThree();
        //Day17.RunPartFour();
        Day17.RunPartFive();
        Console.WriteLine(25295878227269 * 8 + 2);
        Day17.TestA2(25295878227269 * 8 + 2);
        break;
    
    case 18:
        Console.WriteLine("\nDay 18\n");
        Day18.Run();
        break;
}