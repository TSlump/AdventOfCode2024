using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode2024.Days.Day6;

public class Guard
{
    public (int, int) Position { get; set; }
    
    public new Matrix<double> Direction { get; set; }

    public List<((int, int), Matrix<double>)> Path { get; set; }
    
    public bool HasLeft { get; set; }
    
    public string[,] Map { get; set; }
    
    public bool IsInALoop { get; set; }
    

    public Guard()
    {
        var directionMatrix = Matrix<double>.Build.DenseOfArray(new double[,] {
            { 0}, // x
            { 1} // y
        });
        
        this.Direction = directionMatrix;
        this.HasLeft = false;
        this.Path = new List<((int, int) position, Matrix<double> direction)>();
        this.IsInALoop = false;
    }

    public (int, int) IncrementPosition()
    {
        var nextPosition = (Convert.ToInt32(Position.Item1 - Direction[1,0]), Convert.ToInt32(Position.Item2 + Direction[0,0]));

        try
        {
            var test = Map[nextPosition.Item1, nextPosition.Item2];
        }
        catch (IndexOutOfRangeException e)
        {
            HasLeft = true;
            return Position;
        }
        
        if (Map[nextPosition.Item1, nextPosition.Item2] == "#" || Map[nextPosition.Item1, nextPosition.Item2] == "O")
        {
            Rotate();
            Path.Add((Position, Direction));
            
            return Position;
        }
        
        Position = nextPosition;

        if (Path.Contains((Position, Direction)))
        {
            IsInALoop = true;
        }
            
        Path.Add((Position, Direction));

        return Position;
    }

    public void Rotate()
    {
        var rotationMatrix = Matrix<double>.Build.DenseOfArray(new double[,] {
            { 0, 1},
            { -1, 0}
        });
        
        Direction = rotationMatrix * Direction;
    }

    public void ResetAttributes((int, int) originalPosition, Matrix<double> originalDirection)
    {
        Direction = originalDirection;
        Position = originalPosition;
        Path = new List<((int, int), Matrix<double>)>();
        HasLeft = false;
        IsInALoop = false;
    }
}