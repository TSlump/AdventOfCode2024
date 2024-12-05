namespace AdventOfCode2024.Days.Day5;

public class Day5
{
    public static void Run()
    {
        var filePath = "Days/Day5/Day5Input.txt";

        var rules = new List<(int, int)>();
        
        var updates = new List<List<int>>();

        var rulesSection = true;
            
        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                if (line == "")
                {
                    rulesSection = false;
                }
                else if (rulesSection)
                {
                    var pages = line.Split('|');
                    rules.Add((int.Parse(pages[0]), int.Parse(pages[1])));
                }
                else
                {
                    updates.Add(line.Split(',').Select(int.Parse).ToList());
                }
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }

        var correctMiddlePageTotal = 0;
        var incorrectMiddlePageTotal = 0;
        
        foreach (var update in updates)
        {
            var relevantRules = GetRelevantRules(update, rules);
            
            if (IsCorrectUpdate(update, relevantRules))
            {
                correctMiddlePageTotal += update[update.Count/2]; 
            }
            else
            {
                var correctUpdate = CorrectOrderOfUpdate(update, relevantRules);
                
                incorrectMiddlePageTotal += correctUpdate[correctUpdate.Count/2];
            }
        }
        
        Console.WriteLine($"Correctly ordered middle page total: {correctMiddlePageTotal}");
        Console.WriteLine($"Incorrectly ordered middle page total: {incorrectMiddlePageTotal}");
        
        
    }

    public static bool IsCorrectUpdate(List<int> update, List<(int, int)> rules)
    {
        for (var i = 0; i < update.Count; i++)
        {
            foreach (var rule in rules)
            {
                if (update[i] == rule.Item1 && update.GetRange(0, i).Contains(rule.Item2))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static List<(int, int)> GetRelevantRules(List<int> update, List<(int, int)> rules)
    {
        var relevantRules = new List<(int, int)>();

        foreach (var rule in rules)
        {
            if (update.Contains(rule.Item1) && update.Contains(rule.Item2))
            {
                relevantRules.Add((rule.Item1, rule.Item2));
            }
        }
        
        return relevantRules;
    }

    public static List<int> CorrectOrderOfUpdate(List<int> update, List<(int, int)> rules)
    {
        var updateCopy = update.ToList();
        var correctUpdate = new List<int>();

        for (var i = 0; i < update.Count; i++)
        {
            foreach (var page in updateCopy)
            {
               if (WillFollowRules(page, updateCopy, rules))
               {
                   correctUpdate.Add(page);
                   updateCopy.Remove(page);
                   break;
               }
            }
            
        }
        return correctUpdate;
    }

    public static bool WillFollowRules(int page, List<int> futurePages, List<(int, int)> rules)
    {
        foreach (var futurePage in futurePages)
        {
            foreach (var rule in rules)
            {
                if (rule.Item1 == futurePage && rule.Item2 == page)
                {
                    return false;
                }
            }
        }

        return true;
    }
}