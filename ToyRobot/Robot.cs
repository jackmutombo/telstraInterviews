namespace ToyRobot;

public class Robot
{
    public Robot(int X, int Y, Direction direction)
    {
        Position = new Position{X = X, Y = Y};
        Direction = direction;
    }
    public Position Position { get; set; }
    public Direction Direction { get; set; }
}
