namespace ToyRobot;

public static class RobotAppHelper
{
    // Define a method called MoveRobot that takes in a Robot object as a parameter
    public static void MoveRobot(Robot robot)
    {
        // Get the current X and Y position of the robot
        int x = robot.Position.X;
        int y = robot.Position.Y;

        // Determine the direction the robot is facing and adjust its X or Y position accordingly
        switch (robot.Direction)
        {
            case Direction.NORTH:
            case Direction.SOUTH:
                y += GetMoveDirection(robot.Direction); // if facing north or south, adjust Y position
                break;

            case Direction.EAST:
            case Direction.WEST:
                x += GetMoveDirection(robot.Direction); // if facing east or west, adjust X position
                break;
        }

        // Check if the new position is valid
        if (IsValidMove(x, y))
        {
            // Update the robot's position with the new X and Y values
            robot.Position = new Position { X = x, Y = y };
        }
    }

    // Check if the x and y coordinates are within the boundaries of a 6x6 board
    // also greater than or equal to 0 and less than 6 (i.e., within the vertical range of the board).
    public static bool IsValidMove(int x, int y)
    {
        return x >= 0 && x < 6 && y >= 0 && y < 6;
    }

    public static List<string> GetValidCommand()
    {
        var actions = new List<string> { "PLACE", "MOVE", "REPORT", "LEFT", "RIGHT", "EXIT", "CLEAR" };
        actions.AddRange(GetDirectionCmd());
        actions.AddRange(GetBoxStopCMD());
        return actions;
    }

    public static List<string> GetDirectionCmd()
    {
        return new List<string> { "NORTH", "SOUTH", "EAST", "WEST" };
    }

    public static List<string> GetBoxStopCMD()
    {
        var box = new List<string>();
        for (int i = 0; i < 6; i++)
        {
            box.Add(i.ToString());
        }
        return box;
    }
    public static bool IsValidCommand(string command)
    {
        return GetValidCommand().Contains(command);
    }

    public static bool IsValidateBoardPlace(string text)
    {
        // check if place command has more than 14 chars
        if (text.Trim().Length < 14) return false;

        var token = text.Trim().Split(" ");

        // check if the first token is the work PLACE
        if (token[0] != "PLACE") return false;

        var subToken = token[1].Trim().Split(",");

        // check if there more text added and exit
        if (subToken.Length != 3) return false;

        // check if the first coordinate X is in between 0 and 5
        if (!GetBoxStopCMD().Contains(subToken[0])) return false;

        // check if the second coordinate Y is in between 0 and 5
        if (!GetBoxStopCMD().Contains(subToken[1])) return false;

        // check if the facing direction is valid word
        if (!GetDirectionCmd().Contains(subToken[2])) return false;

        return true;
    }

    public static bool IsValidateBoardPlace(string text, bool ignoreDirection = false)
    {
        // check if place command has more than 14 chars
        if (text.Trim().Length < 8) return false;

        var token = text.Trim().Split(" ");

        // check if the first token is the work PLACE
        if (token[0] != "PLACE") return false;

        var subToken = token[1].Trim().Split(",");
        // check if there more text added and exit
        if (subToken.Length != 2 && subToken.Length != 3) return false;

        // check if the first coordinate X is in between 0 and 5
        if (!GetBoxStopCMD().Contains(subToken[0])) return false;

        // check if the second coordinate Y is in between 0 and 5
        if (!GetBoxStopCMD().Contains(subToken[1])) return false;

        // check if the facing direction is valid word
        if (subToken.Length == 3 && !GetDirectionCmd().Contains(subToken[2])) return false;

        return true;
    }

    public static Robot PlaceRobot(string text, Robot robot = null)
    {
        // split the input text into a array of string
        var token = text.Trim().Split(" ");

        var subToken = token[1].Trim().Split(",");

        int x = int.Parse(subToken[0]);
        int y = int.Parse(subToken[1]);

        if (robot != null && subToken.Length == 2)
        {
            robot.Position.X = x;
            robot.Position.Y = y;
            return robot;
        }
        Direction direction = GetDirection(subToken[2]);

        return new Robot(x, y, direction);
    }

    public static void handleCommand(Robot robot, string cmd)
    {
        var cmdTrimmed = cmd.Trim();
        switch (cmdTrimmed)
        {
            case "MOVE":
                MoveRobot(robot);
                return;
            case "LEFT":
            case "RIGHT":
                TurnRobotDirection(robot, GetTurn(cmdTrimmed));
                return;
            case "REPORT":
                Report(robot);
                return;
            case "CLEAR":
                Console.Clear();
                return;
            default:
                if (!cmdTrimmed.StartsWith("PLACE")) return;
                if (IsValidateBoardPlace(cmdTrimmed))
                {
                    robot = PlaceRobot(cmdTrimmed);
                }
                Console.WriteLine($"Invalid command: {cmd}");
                return;

        }
    }

    public static Direction GetDirection(string directionString)
    {
        if (Enum.TryParse(directionString, out Direction direction))
        {
            return direction;
        }
        else
        {
            throw new ArgumentException($"Invalid direction: {directionString}");
        }
    }

    public static Turn GetTurn(string turnString)
    {
        if (Enum.TryParse(turnString, out Turn turn))
        {
            return turn;
        }
        else
        {
            throw new ArgumentException($"Invalid turn: {turnString}");
        }
    }

    public static void PrintIntroCommand()
    {
        Console.WriteLine("\nCandidate Name: Jacques Mutombo Mbuyamba\n");
        Console.WriteLine("------ Welcome to my solution ----------");
        Console.WriteLine("\n**Toy Robot**\n");
        Console.WriteLine("Below the available list of valid command key you may use: ");
        Console.Write("   => ");

        foreach (var cmd in GetValidCommand())
        {
            Console.Write("\'");
            Console.Write(cmd);
            Console.Write("\'");
            Console.Write(" ");
        }
        Console.WriteLine("\nNote: To exit Enter EXIT\n");
        Console.WriteLine("\nEnter a Command below:\n");

    }

    public static int GetMoveDirection(Direction direction)
    {
        if (_directionDictValues.ContainsKey(direction))
        {
            return _directionDictValues[direction];
        }
        else
        {
            throw new ArgumentException("Invalid direction");
        }
    }
    private static Direction TurnDirection(Direction direction, Turn turn)
    {
        if (_directionDictValues.ContainsKey(direction))
        {
            switch (turn)
            {
                case Turn.LEFT:
                    return _leftTurnDictValues[direction];
                case Turn.RIGHT:
                    return _rightTurnDictValues[direction];
                default:
                    throw new ArgumentException("Invalid turn");
            }
        }
        else
        {
            throw new ArgumentException("Invalid direction");
        }
    }

    public static void TurnRobotDirection(Robot robot, Turn turnMove)
    {
        var turnResult = TurnDirection(robot.Direction, turnMove);
        robot.Direction = turnResult;
    }

    public static void Report(Robot robot)
    {
        Console.Write("Output: ");
        Console.Write(robot.Position.X);
        Console.Write(",");
        Console.Write(robot.Position.Y);
        Console.Write(",");
        Console.WriteLine(robot.Direction);
        Console.WriteLine("\n");
    }

    private static Dictionary<Direction, int> _directionDictValues = new Dictionary<Direction, int>
    {
        {Direction.NORTH, 1},
        {Direction.EAST, 1},
        {Direction.SOUTH, -1},
        {Direction.WEST, -1},

    };
    private static Dictionary<Direction, Direction> _leftTurnDictValues = new Dictionary<Direction, Direction>
    {
        {Direction.NORTH, Direction.WEST},
        {Direction.WEST, Direction.SOUTH},
        {Direction.SOUTH, Direction.EAST},
        {Direction.EAST, Direction.NORTH},

    };
    private static Dictionary<Direction, Direction> _rightTurnDictValues = new Dictionary<Direction, Direction>
    {
        {Direction.NORTH, Direction.EAST},
        {Direction.EAST, Direction.SOUTH},
        {Direction.SOUTH, Direction.WEST},
        {Direction.WEST, Direction.NORTH},

    };


}
