using System;
using System.Collections.Generic;
using MartianRobots;
using System.Linq;

namespace console_app
{
    public class Program
    {
        public static char[,]  matrix;
        public static Dictionary<char, int> directions;
        public static Dictionary<char, int> moves;
        public static Scent scent;

        public static int PlanetCoordinateX;
        public static int PlanetCoordinateY;

        public
        static void Main(string[] args)
        {
            var input = "5 3\n1 1 E\nR F R F R F R F\n3 2 N\nF R R F L L F F R R F L L\n0 3 W\nL L F F F L F L F L";
            //var input = "5 5\n1 2 N\nL F L F L F L F F\n3 3 E\nF F R F F R F R R F";
            
            if(!InputValidator.IsValid(input))
            {
                Console.WriteLine("Not able to continue. Input is not valid.");
            }
            
            var splitInput = input.Split('\n');
            var PlanetCoordinates = splitInput[0].Split(' ');
            var planet = new Planet(Int16.Parse(PlanetCoordinates[0]), Int16.Parse(PlanetCoordinates[1]));
            PlanetCoordinateX= planet.CoordenateX;
            PlanetCoordinateY= planet.CoordenateY;

            var tempRobots = splitInput.ToList();
            tempRobots.RemoveAt(0);
            var temp = ListToDictionary(tempRobots.ToList());

            var robots = new List<Robot>();
            
            foreach (KeyValuePair<string,string> result in temp)
            {
                Console.WriteLine(string.Format("Key-{0}:Value-{1}",result.Key,result.Value));
                var positionInput = result.Key.Split(' ');
                var position = new Position(Int16.Parse(positionInput[0]), Int16.Parse(positionInput[1]), positionInput[2].ToString());
                var robot = new Robot(position, result.Value);
                robots.Add(robot);
            }

            populateDirections();
            populateMoves();
            buildDirectionMatrix(moves.Count(), directions.Count());
            scent = new Scent();

            int[] updatedLoc = new int[2];
            var newOrientation = new char();

            foreach (var robot in robots)
            {                
                var orientation = robot.RobotPosition.Orientation;
                var instructions = robot.Instruction.Split(' ');

                for(int i= 0; i< instructions.Length; i++)
                {
                    newOrientation = getUpdateDirection(Char.Parse(instructions[i]), Char.Parse(robot.RobotPosition.Orientation));
                    robot.RobotPosition.Orientation = newOrientation.ToString();

                    if(instructions[i] == "F")
                    {
                        try
                        {
                            updatedLoc = getUpdateLocation(robot.RobotPosition.CoordenateX, robot.RobotPosition.CoordenateY, robot.RobotPosition.Orientation);
                            robot.RobotPosition.CoordenateX = updatedLoc[0];
                            robot.RobotPosition.CoordenateY = updatedLoc[1];
                        }
                        catch (ScentFoundException)
                        {
                           Console.WriteLine("Scent found");
                           continue;
                        }
                        catch (RobotLostException)
                        {
                           Console.WriteLine("Robot Lost");
                           i = instructions.Length;
                        }

                    }

                    //Console.WriteLine("Updating location: (" + robot.RobotPosition.CoordenateX.ToString() + ", " + robot.RobotPosition.CoordenateY + ", " + newOrientation + ")");
                }

                Console.WriteLine("Updated location: Robot#" + robots.IndexOf(robot) + "(" + robot.RobotPosition.CoordenateX.ToString() + ", " + robot.RobotPosition.CoordenateY + ", " + newOrientation + ")");
            }          
        }

    private static int[] getUpdateLocation(int x, int y, string orientation) {

        switch (orientation) {
            case "N":
                y += 1;
                break;
            case "S":
                y -= 1;
                break;
            case "E":
                x += 1;
                break;
            case "W":
                x -= 1;
                break;
        }

        if (x < 0 || y < 0 || x > PlanetCoordinateX || y > PlanetCoordinateY)
        {
            if(!scent.Scents.ContainsKey("Scent"+ x.ToString() + y.ToString()))
            {
                scent.Scents.Add("Scent" + x.ToString() + y.ToString(), x.ToString() + y.ToString());
            }

            throw new RobotLostException();                   
        }

        if(scent.Scents.ContainsKey("Scent"+x.ToString() + y.ToString()))
        {
            throw new ScentFoundException();
        }

        int[] loc = new int[2];
        loc[0] = x;
        loc[1] = y;
        return loc;
    }

    private static Dictionary<T, T> ListToDictionary<T>(IEnumerable<T> a)
    {
        var keys = a.Where((s, i) => i % 2 == 0);
        var values = a.Where((s, i) => i % 2 == 1);
        return keys
            .Zip(values, (k, v) => new KeyValuePair<T, T>(k, v))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }

        private static void buildDirectionMatrix(int row, int col)
        {
            matrix = new char[row,col];
            matrix[0,0] = 'W';
            matrix[0,1] = 'E';
            matrix[0,2] = 'N';
            matrix[0,3] = 'S';
            matrix[1,0] = 'E';
            matrix[1,1] = 'W';
            matrix[1,2] = 'S';
            matrix[1,3] = 'N';
            matrix[2,0] = 'N';
            matrix[2,1] = 'S';
            matrix[2,2] = 'E';
            matrix[2,3] = 'W';

        }
    private static void populateMoves() 
    {
        moves = new Dictionary<char, int>();
        moves.Add('L', 0);
        moves.Add('R', 1);
        moves.Add('F', 2);
    }
    private static void populateDirections() 
    {
        directions = new Dictionary<char,int>();
        directions.Add('N', 0);
        directions.Add('S', 1);
        directions.Add('E', 2);
        directions.Add('W', 3);
    }

    private static char getUpdateDirection(char movement, char oldDirection) {
        char updatedDir = matrix[moves[movement], directions[oldDirection]];
        return updatedDir;
    }

    }
}
