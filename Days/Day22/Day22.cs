using System.Numerics;
using System.Runtime.InteropServices.ComTypes;

namespace AdventOfCode2024.Days.Day22;

public class Day22
{
    public static void Run()
    {
        var filePath = "Days/Day22/Day22Input.txt";
        string[] input = [];

        if (File.Exists(filePath))
        {
            input = File.ReadAllLines(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }

        BigInteger secretNumber = 0;
        
        BigInteger totalSecretNumber = 0;
        
         foreach (var line in input)
         {
             secretNumber = BigInteger.Parse(line);
             
             for (var i = 0; i < 2000; i++)
             {
                 secretNumber = MixAndPrune(secretNumber);
             }
             
             //Console.WriteLine(secretNumber);
             
             totalSecretNumber += secretNumber;
         }
         
         Console.WriteLine($"Total secret number: {totalSecretNumber}");
         
         
         
         
         
         secretNumber = 0;
         
         //var sequenceToSpot = new int[] { -2, 1, -1, 3 };

         var bananasEarnt = new Dictionary<long, (long, bool)>();
         
         foreach (var line in input)
         {
             secretNumber = BigInteger.Parse(line);

             var first = 0;
             var second = 0;
             var third = 0;
             var fourth = 0;
             var fifth = (int) BigInteger.Parse(line) % 10;
              
             for (var i = 0; i < 2000; i++)
             {
                 secretNumber = MixAndPrune(secretNumber);

                 first = second;
                 second = third;
                 third = fourth;
                 fourth = fifth;
                 fifth = (int)(secretNumber % 10);

                 if (i < 3)
                 {
                     continue;
                 }

                 var hashValue = (long)(second - first + 9) + (third - second + 9) * 19 + (fourth - third + 9) * 361 + (fifth - fourth + 9) * 6859;

                 if (bananasEarnt.ContainsKey(hashValue))
                 {
                     if (!bananasEarnt[hashValue].Item2)
                     {
                         bananasEarnt[hashValue] = (bananasEarnt[hashValue].Item1 + fifth % 10, true);
                     }
                 }
                 else
                 {
                     bananasEarnt[hashValue] = (fifth % 10, true);
                 }
                 
             }

             foreach (var bananas in bananasEarnt)
             {
                 bananasEarnt[bananas.Key] = (bananas.Value.Item1, false);
             }
         }

         long maxBananasEarnt = 0;
         long bestHashValue = 0;
         
         foreach (var bananas in bananasEarnt)
         {
             if (bananas.Value.Item1 > maxBananasEarnt)
             {
                 maxBananasEarnt = bananas.Value.Item1;
                 bestHashValue = bananas.Key;
             }
             var currentSequence = new long[] { bananas.Key % 19 - 9, bananas.Key / 19 % 19 - 9, bananas.Key / 361 % 19 - 9, bananas.Key / 6859 % 19 - 9};
             Console.WriteLine($"{string.Join(" ", currentSequence)}: {bananas.Value.Item1}");
         }

         var bestSequence = new long[] { bestHashValue % 19 - 9, bestHashValue / 19 % 19 - 9, bestHashValue / 361 % 19 - 9, bestHashValue / 6859 % 19 - 9};
         
         Console.WriteLine($"Bananas Earnt: {maxBananasEarnt} with Hash: {string.Join(" ", bestSequence)}");
    }

    public static BigInteger MixAndPrune(BigInteger secretNumber)
    {
        var n = secretNumber * 64;

        secretNumber = Mix(secretNumber, n);
        
        secretNumber = Prune(secretNumber);

        n = secretNumber / 32;
        
        secretNumber = Mix(secretNumber, n);
        
        secretNumber = Prune(secretNumber);
        
        n = secretNumber * 2048;
        
        secretNumber = Mix(secretNumber, n);
        
        secretNumber = Prune(secretNumber);
        
        return secretNumber;
    }

    public static BigInteger Mix(BigInteger secretNumber, BigInteger n)
    {
        return n ^ secretNumber;
    }

    public static BigInteger Prune(BigInteger secretNumber)
    {
        return secretNumber % 16777216;
    }
}