namespace AdventOfCode2024.Days.Day17;

public class Day17
{
    public static void Run()
    {
        var filePath = "Days/Day17/Day17Input.txt";
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

    public static (int A, int B, int C, int[] operands) TransformInput(string[] input)
    {
        var A = int.Parse(input[0].Split(" ").Last());
        var B = int.Parse(input[1].Split(" ").Last());
        var C = int.Parse(input[2].Split(" ").Last());
        
        var operands = input[4].Split(" ")[1].Split(",").Select(int.Parse).ToArray();
        
        return (A, B, C, operands);
    }

    public static List<int> RunProgram(int A, int B, int C, int[] operands)
    {
        var output = new List<int>();
        var instructionPointer = 0;
        var jumped = false;

        while (instructionPointer + 1 < operands.Length)
        {
            jumped = false;
            
            var opcode = operands[instructionPointer];
            var operand = operands[instructionPointer + 1];

            var result = 0.0;

            switch (opcode)
            {
                case 0:
                    operand = ComboOperand(A, B, C, operand);
                    
                    result = A / Math.Pow(2, operand);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {result}");

                    A = Convert.ToInt32(Math.Floor(result));
                    break;
                
                case 1:
                    B = (B ^ operand);
                    
                    //Console.WriteLine($"Opcode: {opcode}, Operand: {operand}, Result: {B}");
                    break;
                
                case 2:
                    operand = ComboOperand(A, B, C, operand);

                    B = operand % 8;
                    
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
                    B ^= C;
                    
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

                    C = Convert.ToInt32(Math.Floor(result));
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

    public static int ComboOperand(int A, int B, int C, int operand)
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
                output.Add((int)(B % 5));
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
}