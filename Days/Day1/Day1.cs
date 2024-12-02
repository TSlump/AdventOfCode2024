namespace AdventOfCode2024.Days.Day1;

public class Day1
{
    public static void Run()
    {
        var list1 = new List<int>();
        var list2 = new List<int>();
    
        var filePath = "Days/Day1/Day1Input.txt";
            
        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var locationIds = line.Split("   ");
            
                list1.Add(Convert.ToInt32(locationIds[0]));
                list2.Add(Convert.ToInt32(locationIds[1]));
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }
    
        list1.Sort();
        list2.Sort();
    
        var listDifference = 0;
    
        if (list1.Count == list2.Count)
        {
            for (var i = 0; i < list1.Count; i++)
            {
                listDifference += Math.Abs(list1[i] - list2[i]);
            }
        }
    
        Console.WriteLine("list difference: " + listDifference);
    
        var listSimilarity = 0;
    
        for (var i = 0; i < list1.Count; i++)
        {
            var locationId = list1[i];
            var count = list2.Count(x => x == locationId);
    
            listSimilarity += count * locationId;
        }
    
        Console.WriteLine("list similarity: " + listSimilarity);
    }
    
}