namespace AdventOfCode2024.Days.Day13;

public class Day13
{
    public static void Run()
    {
        var filePath = "Days/Day13/Day13Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        var games = new List<((double, double) prize, ((int, int) a, (int, int) b) buttons)>();

        for (var i = 0; i < (input.Length + 1) / 4; i++)
        {
            var splitA = input[4 * i].Split('+');
            var a = (int.Parse(splitA[1].Split(',')[0]), int.Parse(splitA[2]));
            
            var splitB = input[4 * i + 1].Split('+');
            var b = (int.Parse(splitB[1].Split(',')[0]), int.Parse(splitB[2]));
            
            var splitPrize = input[4 * i + 2].Split('=');
            var prize = (Convert.ToDouble(splitPrize[1].Split(',')[0]) + 10000000000000, Convert.ToDouble(splitPrize[2]) + 10000000000000);
            
            games.Add((prize, (a, b)));
        }
        
        foreach (var game in games)
        {
            Console.WriteLine($"Prize: ({game.prize.Item1}, {game.prize.Item2})");
            Console.WriteLine($"  Buttons:");
            Console.WriteLine($"    A: ({game.buttons.a.Item1}, {game.buttons.a.Item2})");
            Console.WriteLine($"    B: ({game.buttons.b.Item1}, {game.buttons.b.Item2})");
            Console.WriteLine();
        }

        var totalTokens = 0;

        var APresses = 0;
        var validSolution = true;

        foreach (var game in games)
        {
            APresses = 0;
            validSolution = true;
            
            var prize = (game.prize.Item1, game.prize.Item2);

            while (prize.Item1 % game.buttons.b.Item1 != 0 ||
                   prize.Item2 % game.buttons.b.Item2 != 0 || 
                   prize.Item1 / game.buttons.b.Item1 != prize.Item2 / game.buttons.b.Item2)
            {
                prize.Item1 -= game.buttons.a.Item1;
                prize.Item2 -= game.buttons.a.Item2;
                APresses++;

                if (prize.Item1 < 0 || prize.Item2 < 0)
                {
                    Console.WriteLine("Invalid Solution");
                    validSolution = false;
                    break;
                }
            }

            if (validSolution)
            {
               Console.WriteLine($"{APresses}, {prize.Item1 / game.buttons.b.Item1}");
               totalTokens += 3 * APresses + Convert.ToInt32(prize.Item1 / game.buttons.b.Item1); 
            }
        }
        
        Console.WriteLine($"Total tokens: {totalTokens}");

        
        
    }
}