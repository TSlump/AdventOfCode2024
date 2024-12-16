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
               
               
               
               Console.WriteLine($"Prize: ({game.prize.Item1}, {game.prize.Item2}): A Presses: {APresses}, B Presses: {prize.Item1 / game.buttons.b.Item1}");
            }
        }
        
        Console.WriteLine($"Total tokens: {totalTokens}");
    }


    public static void RunPartTwo()
    {
        var filePath = "Days/Day13/Day13TestInput2.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        var games = new List<((long, long) prize, ((long, long) a, (long, long) b) buttons)>();

        for (var i = 0; i < (input.Length + 1) / 4; i++)
        {
            var splitA = input[4 * i].Split('+');
            var a = (Convert.ToInt64(splitA[1].Split(',')[0]), Convert.ToInt64(splitA[2]));

            var splitB = input[4 * i + 1].Split('+');
            var b = (Convert.ToInt64(splitB[1].Split(',')[0]), Convert.ToInt64(splitB[2]));

            var splitPrize = input[4 * i + 2].Split('=');
            var prize = (Convert.ToInt64(splitPrize[1].Split(',')[0]) + 10000000000000,
                Convert.ToInt64(splitPrize[2]) + 10000000000000);
            // var prize = (Convert.ToInt64(splitPrize[1].Split(',')[0]),
            //     Convert.ToInt64(splitPrize[2]));

            games.Add((prize, (a, b)));
        }
        
        Console.WriteLine();
        Console.WriteLine();

        var APresses = 0.0;
        var BPresses = 0.0;

        long totalTokens2 = 0;

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

                d.Item2 *= (long) (game2.prize.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1));
                d.Item3 *= (long) (game2.prize.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1));

                Console.WriteLine($"Adjusted for price: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                if (d.Item3 < 0)
                {
                    var adjuster = (long) Math.Ceiling((double)(-d.Item3 / game2.buttons.a.Item1));

                    d.Item3 += game2.buttons.a.Item1 * adjuster;
                    d.Item2 -= game2.buttons.b.Item1 * adjuster;
                }

                if (d.Item2 < 0)
                {
                    var adjuster = (long)Math.Ceiling((double)(-d.Item2 / game2.buttons.b.Item1));

                    d.Item2 += game2.buttons.b.Item1 * adjuster;
                    d.Item3 -= game2.buttons.a.Item1 * adjuster;
                }

                Console.WriteLine($"Made positive: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                long reducer = (long)Math.Floor(d.Item2 / (game2.buttons.b.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1)));
                d.Item2 -= (long)(game2.buttons.b.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1)) * reducer;
                d.Item3 += (long)(game2.buttons.a.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1)) * reducer;

                Console.WriteLine($"Reduced A: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                APresses = d.Item2;
                BPresses = d.Item3;

                Console.WriteLine();

                Console.WriteLine($"    y: {game2.buttons.a.Item2}, {game2.buttons.b.Item2}");

                d = extended_euclidean(game2.buttons.a.Item2, game2.buttons.b.Item2);
                Console.WriteLine($"Initial inverse: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                d.Item2 *= (long)(game2.prize.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2));
                d.Item3 *= (long)(game2.prize.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2));

                Console.WriteLine($"Adjusted for prize: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                if (d.Item3 < 0)
                {
                    var adjuster = (long)Math.Ceiling((double)-d.Item3 / game2.buttons.a.Item2);

                    d.Item3 += game2.buttons.a.Item2 * adjuster;
                    d.Item2 -= game2.buttons.b.Item2 * adjuster;
                }

                if (d.Item2 < 0)
                {
                    var adjuster = (long)Math.Ceiling((double)-d.Item2 / game2.buttons.b.Item2);

                    d.Item2 += game2.buttons.b.Item2 * adjuster;
                    d.Item3 -= game2.buttons.a.Item2 * adjuster;
                }

                Console.WriteLine($"Made positive: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");

                reducer = (long)Math.Floor(d.Item2 / (game2.buttons.b.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2)));
                d.Item2 -= (long)(game2.buttons.b.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2)) * reducer;
                d.Item3 += (long)(game2.buttons.a.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2)) * reducer;

                Console.WriteLine($"Reduced A: x: {d.Item2}, y: {d.Item3}, d: {d.Item1}");
                
                for (var x = 0; x < 100000; x++)
                {
                    var gcdForY = GCD(game2.buttons.a.Item1, game2.buttons.b.Item1);
                    var gcdForX = GCD(game2.buttons.a.Item2, game2.buttons.b.Item2);
                    
                    double y =  (APresses - d.Item2 + x * (game2.buttons.b.Item1 / gcdForY)) / (game2.buttons.b.Item2 / gcdForX);
                   
                    

                   if (Math.Abs(d.Item2 + y * (game2.buttons.b.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2)) - (APresses + x * (game2.buttons.b.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1)))) < 0.1 &&
                       Math.Abs(d.Item3 - y * (game2.buttons.a.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2)) - (BPresses - x * (game2.buttons.a.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1)))) < 0.1)
                   {
                       Console.WriteLine($"y: {y}, x: {x}");
                       Console.WriteLine($"new solutions: A Presses: {d.Item2 + y * (game2.buttons.b.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2))}," +
                                        $" B Presses: {d.Item3 - y * (game2.buttons.a.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2))}");
                      
                      Console.WriteLine($"solutions check: A Presses: {APresses + x * (game2.buttons.b.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1))}, " +
                                        $"B Presses: {BPresses - x * (game2.buttons.a.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1))}");
                       
                       
                       totalTokens2 += (long)(3 * (d.Item2 + y * (game2.buttons.b.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2))) +
                                       (d.Item3 - y * (game2.buttons.a.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2))));
                       
                       Console.WriteLine($"Currenty Tokens: {totalTokens2}");

                       break;
                   }

                   if (d.Item3 - y * (game2.buttons.a.Item2 / GCD(game2.buttons.a.Item2, game2.buttons.b.Item2)) < 0 || BPresses - x * (game2.buttons.a.Item1 / GCD(game2.buttons.a.Item1, game2.buttons.b.Item1)) < 0)
                   {
                       Console.WriteLine($"Currenty Tokens: {totalTokens2}");
                       break;
                   }
                }
            }
            else
            {
                Console.WriteLine("Invalid Solution");
            }
        }

        Console.WriteLine($"Total Tokens: {totalTokens2}");
    }
    
    public static double GCD(long p, long q)
    {
        if(q == 0)
        {
            return p;
        }

        long r = p % q;

        return GCD(q, r);
    }

    public static (long gcd, long x, long y) extended_euclidean(long a, long b)
    {
        if (b == 0)
        {
            return (a, 1, 0);
        }
        
        var (gcd, x1, y1) = extended_euclidean(b, a % b);

        long x = y1;
        long y = x1 - (a / b) * y1;
        
        return (gcd, x, y);
    }

    public static void LineIntersection()
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

        var games = new List<((double x, double y) prize, ((double x, double y) a, (double x, double y) b) buttons)>();

        for (var i = 0; i < (input.Length + 1) / 4; i++)
        {
            var splitA = input[4 * i].Split('+');
            var a = (Convert.ToDouble(splitA[1].Split(',')[0]), Convert.ToDouble(splitA[2]));

            var splitB = input[4 * i + 1].Split('+');
            var b = (Convert.ToDouble(splitB[1].Split(',')[0]), Convert.ToDouble(splitB[2]));

            var splitPrize = input[4 * i + 2].Split('=');
            var prize = (Convert.ToInt64(splitPrize[1].Split(',')[0]) + 10000000000000,
               Convert.ToInt64(splitPrize[2]) + 10000000000000);
            // var prize = (Convert.ToDouble(splitPrize[1].Split(',')[0]),
             //    Convert.ToDouble(splitPrize[2]));

            games.Add((prize, (a, b)));
        }
        
        Console.WriteLine();
        Console.WriteLine();

        var tokens = 0.0;

        foreach (var game in games)
        {
            Console.WriteLine($"Prize: ({game.prize.x}, {game.prize.y})");
            Console.WriteLine($"  Buttons:");
            Console.WriteLine($"    A: ({game.buttons.a.x}, {game.buttons.a.y})");
            Console.WriteLine($"    B: ({game.buttons.b.x}, {game.buttons.b.y})");
            Console.WriteLine();
            
            var prize = game.prize;
            var a = game.buttons.a;
            var b = game.buttons.b;

            var c = prize.y - (b.y / b.x) * prize.x;

            var x = c / (a.y / a.x - b.y / b.x);

            var y = (a.y / a.x) * x;
            
            Console.WriteLine($"    Intercept: ({x}, {y})");

            var APresses = x / a.x;
            var BPresses = (prize.x - x) / b.x;
            
            Console.WriteLine($"  A Presses: {APresses}");
            Console.WriteLine($"  B Presses: {BPresses}");
            Console.WriteLine();

            var roundedAPresses = Math.Round(APresses);
            var roundedBPresses = Math.Round(BPresses);
            
            if (Math.Abs(roundedAPresses - APresses) < 0.001 && Math.Abs(roundedBPresses - BPresses) < 0.001)
            {
                Console.WriteLine($"Tokens Added !!");
                tokens += 3 * Math.Round(APresses) + Math.Round(BPresses);
            }
        }
        
        Console.WriteLine($"Total Tokens: {tokens}");
    }
}