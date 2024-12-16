namespace AdventOfCode2024.Days.Day14;

public class Robot
{
    public (int x, int y) Coords;

    public (int x, int y) Direction;

    public (int x, int y) GridSize;
    
    public Robot((int, int) coords, (int x, int y) direction, (int x, int y) gridSize)
    {
        this.Coords = coords;
        this.Direction = direction;
        this.GridSize = gridSize;
    }

    public void Move()
    {
        Coords.x += Direction.x + GridSize.x;
        Coords.y += Direction.y + GridSize.y;
        
        Coords.x %= GridSize.x;
        Coords.y %= GridSize.y;
    }
}