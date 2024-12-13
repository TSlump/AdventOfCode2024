namespace AdventOfCode2024.Days.Day13;

public class Day13
{
    public static void RunPartOne()
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
            var prize = (Convert.ToDouble(splitPrize[1].Split(',')[0]), Convert.ToDouble(splitPrize[2]));
            
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


    public static void RunPartTwo()
    {
        var filePath = "Days/Day13/Day13TestInput.txt";
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
            // var prize = (Convert.ToDouble(splitPrize[1].Split(',')[0]) + 10000000000000,
            //     Convert.ToDouble(splitPrize[2]) + 10000000000000);
            var prize = (Convert.ToDouble(splitPrize[1].Split(',')[0]),
                Convert.ToDouble(splitPrize[2]));

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
        
        Console.WriteLine();
        Console.WriteLine();

        var APresses = 0.0;
        var BPresses = 0.0;

        var totalTokens2 = 0.0;

        foreach (var game2 in games)
        {
            if (game2.prize.Item1 % GCD(game2.buttons.a.Item1, game2.buttons.b.Item1) == 0 &&
                game2.prize.Item2 % GCD(game2.buttons.a.Item2, game2.buttons.b.Item2) == 0)
            {
                Console.WriteLine($"Prize: ({game2.prize.Item1}, {game2.prize.Item2})");
                Console.WriteLine($"  Buttons:");
                Console.WriteLine($"    A: ({game2.buttons.a.Item1}, {game2.buttons.a.Item2})");
                Console.WriteLine($"    B: ({game2.buttons.b.Item1}, {game2.buttons.b.Item2})");
                Console.WriteLine();

                Console.WriteLine(
                    $"Valid Solution, gcd:{GCD(game2.buttons.a.Item1, game2.buttons.b.Item1)} and {GCD(game2.buttons.a.Item2, game2.buttons.b.Item2)}");

                Console.WriteLine($"    x: {game2.buttons.a.Item1}, {game2.buttons.b.Item1}");


                var d = extended_euclidean(game2.buttons.a.Item1, game2.buttons.b.Item1);
                Console.WriteLine($"Initial inverse: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                d.Item2 *= game2.prize.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1);
                d.Item3 *= game2.prize.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1);

                Console.WriteLine($"Adjusted for price: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                if (d.Item3 < 0)
                {
                    var adjuster = Math.Ceiling(-d.Item3 / game2.buttons.a.Item1);

                    d.Item3 += game2.buttons.a.Item1 * adjuster;
                    d.Item2 -= game2.buttons.b.Item1 * adjuster;
                }

                if (d.Item2 < 0)
                {
                    var adjuster = Math.Ceiling(-d.Item2 / game2.buttons.b.Item1);

                    d.Item2 += game2.buttons.b.Item1 * adjuster;
                    d.Item3 -= game2.buttons.a.Item1 * adjuster;
                }

                Console.WriteLine($"Made positive: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                var reducer = Math.Floor(d.Item2 / Math.Max(game2.buttons.b.Item1, game2.buttons.a.Item1));
                d.Item2 -= game2.buttons.a.Item1 * reducer;
                d.Item3 += game2.buttons.b.Item1 * reducer;

                Console.WriteLine($"Reduced A: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                APresses = d.Item2;
                BPresses = d.Item3;

                Console.WriteLine();

                Console.WriteLine($"    y: {game2.buttons.a.Item2}, {game2.buttons.b.Item2}");

                d = extended_euclidean(game2.buttons.a.Item2, game2.buttons.b.Item2);
                Console.WriteLine($"Initial inverse: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                d.Item2 *= game2.prize.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2);
                d.Item3 *= game2.prize.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2);

                Console.WriteLine($"Adjusted for prize: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                if (d.Item3 < 0)
                {
                    var adjuster = Math.Ceiling(-d.Item3 / game2.buttons.a.Item2);

                    d.Item3 += game2.buttons.a.Item2 * adjuster;
                    d.Item2 -= game2.buttons.b.Item2 * adjuster;
                }

                if (d.Item2 < 0)
                {
                    var adjuster = Math.Ceiling(-d.Item2 / game2.buttons.b.Item2);

                    d.Item2 += game2.buttons.b.Item2 * adjuster;
                    d.Item3 -= game2.buttons.a.Item2 * adjuster;
                }

                Console.WriteLine($"Made positive: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                reducer = Math.Floor(d.Item2 / Math.Max(game2.buttons.b.Item1, game2.buttons.a.Item1));
                d.Item2 -= game2.buttons.b.Item2 * reducer;
                d.Item3 += game2.buttons.a.Item2 * reducer;

                Console.WriteLine($"Reduced A: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                if (d.Item2 == APresses && d.Item3 == BPresses)
                {
                    totalTokens2 += 3 * APresses + BPresses;
                }
            }
            else
            {
                Console.WriteLine("Invalid Solution");
            }
        }

        Console.WriteLine($"Total Tokens: {totalTokens2}");
    }
    
    public static int GCD(int p, int q)
    {
        if(q == 0)
        {
            return p;
        }

        int r = p % q;

        return GCD(q, r);
    }

    public static (int gcd, double x, double y) extended_euclidean(int a, int b)
    {
        if (b == 0)
        {
            return (a, 1, 0);
        }
        
        var (gcd, x1, y1) = extended_euclidean(b, a % b);

        double x = y1;
        double y = x1 - (a / b) * y1;
        
        return (gcd, x, y);
    }
}