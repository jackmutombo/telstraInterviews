// See https://aka.ms/new-console-template for more information
using ToyRobot;
using static ToyRobot.RobotAppHelper;

PrintIntroCommand();


bool exit = false;
bool isPlacedCorrect = false;
Robot robot1 = null;

do
{
    var line = "";
    while (!isPlacedCorrect)
    {
        // this is run only for the first time to place the robot
        Console.Write("> ");
        line = Console.ReadLine();
        isPlacedCorrect = IsValidateBoardPlace(line);
        if (isPlacedCorrect)
        {
            robot1 = PlaceRobot(line);
        }
        if (line.Trim() == "EXIT")
        {
            exit = true;
            break;
        }
        if (line.Trim() == "CLEAR")
        {
            Console.Clear();
        }
        if (!isPlacedCorrect)
        {
            Console.WriteLine("Invalid command, you first need to place the toy robot using the PLACE command. e.g PLACE 0,0,NORTH");
        }
    }

    Console.Write("> ");
    line = Console.ReadLine();
    if (IsValidateBoardPlace(line, true))
    {
        robot1 = PlaceRobot(line, robot1);
    }
    else if (IsValidCommand(line))
    {
        handleCommand(robot1, line);
        if (line.Trim() == "EXIT")
        { exit = true; }
    } else {
        Console.WriteLine($"Invalid command: {line}");
    }
    
} while (!exit);
