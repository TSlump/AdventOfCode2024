namespace AdventOfCode2024.Days.Day9;

public class Day9
{
    public static void Run()
    {
        var filePath = "Days/Day9/Day9Input.txt";
        
        var input = GetInputFromFile(filePath);

        var fileLayout = GenerateFileStructure(input);
        
        fileLayout = CompressFileLayoutSystemically(fileLayout);

        var checkSum = ComputeCheckSum(fileLayout);
        
        Console.WriteLine(checkSum);
    }

    public static string GetInputFromFile(string filePath)
    {
        var input = "";
        
        if (File.Exists(filePath))
        {
            input =  File.ReadAllText(filePath);
        }
        else
        {
            Console.WriteLine("File not found");
        }
        
        return input;
    }

    public static Dictionary<int, string> GenerateFileStructure(string input)
    {
        var isSpace = false;
        var fileLayout = new Dictionary<int, string>();
        var indexCounter = 0;
        var idCounter = 0;

        foreach (var character in input)
        {
            var blockLength = Convert.ToInt32(character.ToString());

            if (isSpace)
            {
                for (var i = 0; i < blockLength; i++)
                {
                    fileLayout[indexCounter] = "."; 
                    indexCounter++;
                    
                }

                isSpace = false;
            }
            else
            {
                for (var i = 0; i < blockLength; i++)
                {
                    fileLayout[indexCounter] = idCounter.ToString();
                    indexCounter++;
                    
                }

                idCounter++;
                isSpace = true;
            }
        }
        
        return fileLayout;
    }

    public static Dictionary<int, string> CompressFileLayout(Dictionary<int, string> fileLayout)
    {
        var indexCounter = 0;
        var reverseIndexCounter = fileLayout.Count - 1;

        while (indexCounter <= reverseIndexCounter)
        {
            if (fileLayout[indexCounter] == ".")
            {
                fileLayout[indexCounter] = fileLayout[reverseIndexCounter];
                fileLayout[reverseIndexCounter] = ".";
                
                reverseIndexCounter--;

                while (fileLayout[reverseIndexCounter] == ".")
                {
                    reverseIndexCounter--;
                }
                
            }
            indexCounter++;
        }
        
        return fileLayout;
    }

    public static Dictionary<int, string> CompressFileLayoutSystemically(Dictionary<int, string> fileLayout)
    {
        var reverseIndexCounter = fileLayout.Count - 1;
        
        while (fileLayout[reverseIndexCounter] != "0")
        {

            if (fileLayout[reverseIndexCounter] == ".")
            {
                reverseIndexCounter--;
            }
            else
            {
                var blockLength = GetBlockLength(fileLayout, reverseIndexCounter);
                
                var indexOfGap = GetIndexOfSuitableGap(fileLayout, blockLength, reverseIndexCounter);
                
                if (indexOfGap != -1)
                {
                    for (var i = 0; i < blockLength; i++)
                    {
                        fileLayout[indexOfGap + i] = fileLayout[reverseIndexCounter - i];
                        fileLayout[reverseIndexCounter - i] = ".";
                    }
                }
                reverseIndexCounter -= blockLength;
            }
            
        }

        return fileLayout;
    }

    public static int GetBlockLength(Dictionary<int, string> fileLayout, int reverseIndexCounter)
    {
        var blockLength = 0;
        var id = fileLayout[reverseIndexCounter];

        while (fileLayout[reverseIndexCounter - blockLength] == id)
        {
            blockLength++;
        }
        
        return blockLength;
    }

    public static int GetIndexOfSuitableGap(Dictionary<int, string> fileLayout, int gapLength, int cutOffIndex)
    {
        var blanksCounter = 0;
        
        for (var i = 0; i < cutOffIndex; i++)
        {
            if (fileLayout[i] == ".")
            {
                blanksCounter++;

                if (blanksCounter == gapLength)
                {
                    return i - gapLength + 1;
                }
            }
            else
            {
                blanksCounter = 0;
            }
        }

        return -1;
    }

    public static double ComputeCheckSum(Dictionary<int, string> fileLayout)
    {
        var checkSum = 0.0;
        
        for (var i = 0; i < fileLayout.Count; i++)
        {
            if (fileLayout[i] != ".")
            {
                checkSum += Convert.ToInt32(fileLayout[i]) * i;
            }
        }
        
        return checkSum;
    }
}