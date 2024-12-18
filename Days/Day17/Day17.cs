using System.Numerics;

namespace AdventOfCode2024.Days.Day17;

public class Day17
{
    public static void Run()
    {
        var filePath = "Days/Day17/Day17TestInput2.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        var (A, B, C, operands) = TransformInput(input);
        
        Console.WriteLine($"A: {A}, B: {B}, C: {C}, Operands: {string.Join(',', operands)}");
        
        var output = RunProgram(A, B, C, operands);
        
        Console.WriteLine($"Output: {string.Join(',', output)}");
        
        Console.WriteLine($"A: {A}, B: {B}, C: {C}, Operands: {string.Join(',', operands)}");

        A = -1;

        // while (!output.SequenceEqual(operands))
        // {
        //     A += 1;
        //     output = RunProgram(A, B, C, operands);
        //
        //     if (A % 1000000 == 0)
        //     {
        //         Console.WriteLine($"A: {A}");
        //     }
        // }
        
        //Console.WriteLine($"Output: {string.Join(',', output)}, A: {A}, B: {B}, C: {C}");
    }

    public static (long A, int B, int C, int[] operands) TransformInput(string[] input)
    {
        var A = Convert.ToInt64(input[0].Split(" ").Last());
        var B = int.Parse(input[1].Split(" ").Last());
        var C = int.Parse(input[2].Split(" ").Last());
        
        var operands = input[4].Split(" ")[1].Split(",").Select(int.Parse).ToArray();
        
        return (A, B, C, operands);
    }

    public static List<int> RunProgram(long A, int B, long C, int[] operands)
    {
        var output = new List<int>();
        long instructionPointer = 0;
        var jumped = false;

        while (instructionPointer + 1 < operands.Length)
        {
            jumped = false;
            
            var opcode = operands[instructionPointer];
            long operand = operands[instructionPointer + 1];

            var result = 0.0;

            switch (opcode)
            {
                case 0:
                    operand = ComboOperand(A, B, C, operand);
                    
                    result = A / Math.Pow(2, operand);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {result}");

                    A = Convert.ToInt64(Math.Floor(result));
                    break;
                
                case 1:
                    B = (int)(B ^ operand);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {B}");
                    break;
                
                case 2:
                    operand = ComboOperand(A, B, C, operand);

                    B = (int)(operand % 8);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {B}");
                    break;
                
                case 3:
                    if (A != 0)
                    {
                        instructionPointer = operand;
                        jumped = true;
                        
                        //Console.WriteLine($"Jumped to: {instructionPointer}");
                    }

                    break;
                
                case 4:
                    B = (int)(B ^ C);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {B}");
                    break;
                
                case 5:
                    operand = ComboOperand(A, B, C, operand);

                    result = operand % 8;
                    
                    output.Add(Convert.ToInt32(result));
                    
                    //Console.WriteLine($"Added to output: {result}");
                    break;
                
                case 6:
                    operand = ComboOperand(A, B, C, operand);
                    
                    result = A / Math.Pow(2, operand);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {result}");

                    B = Convert.ToInt32(Math.Floor(result));
                    break;
                
                case 7:
                    operand = ComboOperand(A, B, C, operand);
                    
                    result = A / Math.Pow(2, operand);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {result}");

                    C = Convert.ToInt64(Math.Floor(result));
                    break;
            }
            
            //Console.WriteLine($"Instruction Pointer: {instructionPointer}, A: {A}, B: {B}, C: {C}");

            if (!jumped)
            {
                instructionPointer += 2;
            }
        }

        return output;
    }

    public static long ComboOperand(long A, int B, long C, long operand)
    {
        return operand switch
        {
            4 => A,
            5 => B,
            6 => C,
            _ => operand
        };
    }

    public static void RunPartTwo()
    {
        long counter = 10000;

        var target = new List<int> {2,4,1,1,7,5,4,7,1,4,0,3,5,5,3,0};
        
        var output = new List<int>();
        
        while (true)
        {
            if (counter % 10000 == 0)
            {
                Console.WriteLine(counter);
            }
            
            output.Clear();
            
            long A = counter;
            long B = 0;
            long C = 0;

            while (A != 0)
            {
                B = A % 8;
                B = B ^ 1;
                C = Convert.ToInt64(Math.Floor(A / Math.Pow(2, B)));
                B = B ^ C;
                B = B ^ 4;
                A = A / 8;
                output.Add((int)(B % 8));
            }

            if (target.SequenceEqual(output))
            {
                break;
            }
            else
            {
                Console.WriteLine($"A: {counter}, Failed output: {string.Join(',', output)}");
                System.Threading.Thread.Sleep(1000);
            }

            counter++;
            
        }
        
        Console.WriteLine(counter);
    }

    public static void TestInputs()
    {
        for (var A = 0; A < 1000; A++)
        {
            long B = 0;
            long C = 0;
            
            B = A % 8;
            B = B ^ 1;
            C = Convert.ToInt64(Math.Floor(A / Math.Pow(2, B)));
            B = B ^ C;
            B = B ^ 4;
            //A = A / 8;
            
            //Console.WriteLine($"A: {Convert.ToString(A, 2)}, Output: {Convert.ToString(B%8, 2)}");

            if ((B % 8) == 2)
            {
                Console.WriteLine($"A: {A}, Output: {(B % 8)}");
                Console.WriteLine($"A: {Convert.ToString(A, 8)}, Output: {Convert.ToString(B % 8, 8)}");
                break;
            }
        }
    }

    public static void RunPartThree()
    {
        var targets = new List<int> {2,4,1,1,7,5,4,7,1,4,0,3,5,5,3,0};

        var oldValues = new List<(int, int, int)>{};

        var newValues = new List<(int, int, int)>{(0, 0, 0)};

        foreach (var target in targets)
        {
            oldValues = newValues.ToList();
            newValues.Clear();
            
            foreach (var oldValue in oldValues)
            {
                 var validList = GetNeededA(target, oldValue.Item1, oldValue.Item2);
                 Console.WriteLine(string.Join(" ", validList.Select(item => $"({item.Item1}, {item.Item2}, {item.Item3})")));
                 
                 
            }

        }
    }

    public static List<(int, int, int)> GetNeededA(int target, int inputB, int inputC)
    {
        var output = new List<(int, int, int)>();
        
        for (var A = 0; A < 8; A++)
        {
            var B = Convert.ToInt32(inputB);
            var C = Convert.ToInt32(inputC);
            
            B = A % 8;
            B = B ^ 1;
            C = Convert.ToInt32(Math.Floor(A / Math.Pow(2, B)));
            B = B ^ C;
            B = B ^ 4;
            if (B % 8 == target)
            {
                output.Add((A, B, C));
            }
        }
        
        return output;
    }

    public static void RunPartFour()
    {
        var ListOfAs = new List<int> { };
        RecursiveFindA(ListOfAs, 0, 0, 0);
        
        Console.WriteLine(string.Join(",", ListOfAs));
        
        Console.WriteLine();

        long input = 0;
        foreach (var A in ListOfAs)
        {
            input *= 64;

            input += A;
        }
        
        Console.WriteLine(input);
    }

    public static bool RecursiveFindA(List<int> ListOfAs, int B, int C, int depth)
    {
        var targets = new List<int> {2,4,1,1,7,5,4,7,1,4,0,3,5,5,3,0};

        if (depth == targets.Count)
        {
            Console.WriteLine($"Depth reached, return true");
            return true;
        }
        
        var neededAs = GetNeededA(targets[depth], B, C);
        
        Console.WriteLine($"Target: {targets[depth]}, B: {B}, C: {C}, Valid As: {string.Join(" next ", neededAs.Select(item => $"({item.Item1}, {item.Item2}, {item.Item3})"))}");

        if (neededAs.Count == 0)
        {
            Console.WriteLine($"No As found, return false");
            return false;
        }

        foreach (var neededA in neededAs)
        {
            if (RecursiveFindA(ListOfAs, neededA.Item2, neededA.Item3, depth + 1))
            {
                Console.WriteLine($"A found, {neededA.Item1} added");
                ListOfAs.Add(neededA.Item1);
                return true;
            }
        }

        return false;
    }

    public static void RunPartFive()
    {
        var targets = new List<int> {2,4,1,1,7,5,4,7,1,4,0,3,5,5,3,0};

        BigInteger A = 0;

        var (_, A2) = RecursiveMakeTree(A, 1);
        
        Console.WriteLine(A2);
    }

    public static (bool, BigInteger) RecursiveMakeTree(BigInteger A, int depth)
    {
        var targets = new List<int> {2,4,1,1,7,5,4,7,1,4,0,3,5,5,3,0};

        if (depth == targets.Count + 1)
        {
            Console.WriteLine($"Depth reached, return true");
            return (true, A);
        }
        
        for (int increment = 0; increment < 8; increment++)
        {
            if (TestA(A* 8 + increment) == targets[^depth])
            {
                Console.WriteLine($"Target: {targets[^depth]}, A: {A}, Increment: {increment}");

                var (isGood, A3) = RecursiveMakeTree(A * 8 + increment, depth + 1);
                if (isGood)
                {
                    return (true, A3*8 + increment);
                }
            }
        }

        return (false, A);
    }

    public static BigInteger TestA(BigInteger A)
    {
        var B = A % 8;
        B ^= 1;
        var C = Convert.ToInt64(Math.Floor((double)(A / (BigInteger)Math.Pow(2, (double)B))));
        B ^= C;
        B ^= 4;

        return B % 8;
    }

    public static void TestA2(BigInteger A)
    {
        while (A > 0)
        {
            Console.WriteLine(RunPartProgram(A));

            A = A / 8;
        }
    }

    public static BigInteger RunPartProgram(BigInteger A)
    {
        var B = A % 8;
        B ^= 1;
        var C = Convert.ToInt64(Math.Floor((double)(A / (BigInteger)Math.Pow(2, (double)B))));
        B ^= C;
        B ^= 4;

        return B % 8;
    }
}