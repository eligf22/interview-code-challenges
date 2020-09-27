using System;
using System.Collections.Generic;
using MartianRobots;
using MartianRobots.Validators;
using MartianRobots.Exceptions;
using MartianRobots.Models;
using System.Linq;

namespace MartianRobots
{
    public class MartianRobotsHandler : IMartianRobotsHandler
    {
        public static char[,]  matrix;
        public static Dictionary<char, int> directions;
        public static Dictionary<char, int> moves;
        public Scent scent;
        public int PlanetCoordinateX;
        public int PlanetCoordinateY;
        public IConsoleWriter CustomConsoleWriter { get; set; }
        public MartianRobotsHandler(IConsoleWriter customConsoleWriter)
        {
            CustomConsoleWriter = customConsoleWriter;
        }

        public void Execute(string input)
        {
            if(!InputValidator.IsValid(input))
            {
                CustomConsoleWriter.WriteLine("Not able to continue. Input is not valid.");
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
                var positionInput = result.Key.Split(' ');
                var position = new Position(Int16.Parse(positionInput[0]), Int16.Parse(positionInput[1]), positionInput[2].ToString());
                var robot = new Robot(position, result.Value);
                robots.Add(robot);
            }
            
            LoadData();
            scent = new Scent();

            int[] updatedLoc = new int[2];
            var newOrientation = new char();

            foreach (var robot in robots)
            {                
                var orientation = robot.RobotPosition.Orientation;
                var instructions = robot.Instruction.ToCharArray();

                for(int i= 0; i< instructions.Length; i++)
                {
                    newOrientation = getUpdateDirection(instructions[i], char.Parse(robot.RobotPosition.Orientation));
                    robot.RobotPosition.Orientation = newOrientation.ToString();

                    if(instructions[i] == 'F')
                    {
                        try
                        {
                            updatedLoc = getUpdateLocation(robot.RobotPosition.CoordenateX, robot.RobotPosition.CoordenateY, robot.RobotPosition.Orientation);
                            robot.RobotPosition.CoordenateX = updatedLoc[0];
                            robot.RobotPosition.CoordenateY = updatedLoc[1];
                        }
                        catch (ScentFoundException)
                        {
                           CustomConsoleWriter.WriteLine("Scent found");
                           continue;
                        }
                        catch (RobotLostException)
                        {
                           CustomConsoleWriter.WriteLine("Robot Lost");
                           i = instructions.Length;
                        }

                    }

                }

                CustomConsoleWriter.WriteLine("Updated location for Robot#" + robots.IndexOf(robot) + "(" + robot.RobotPosition.CoordenateX.ToString() + ", " + robot.RobotPosition.CoordenateY + ", " + newOrientation + ")");
            }  
        }

    #region Private methods
    private int[] getUpdateLocation(int x, int y, string orientation) {

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
                throw new RobotLostException(); 
            }
            else{
                 throw new ScentFoundException();
            }                              
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

    private void buildDirectionMatrix(int row, int col)
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
    private void populateMoves() 
    {
        moves = new Dictionary<char, int>();
        moves.Add('L', 0);
        moves.Add('R', 1);
        moves.Add('F', 2);
    }
    private void populateDirections() 
    {
        directions = new Dictionary<char,int>();
        directions.Add('N', 0);
        directions.Add('S', 1);
        directions.Add('E', 2);
        directions.Add('W', 3);
    }

    private char getUpdateDirection(char movement, char oldDirection) {
        char updatedDir = matrix[moves[movement], directions[oldDirection]];
        return updatedDir;
    }

    private void LoadData()
    {
        populateDirections();
        populateMoves();
        buildDirectionMatrix(moves.Count(), directions.Count());
    }

    #endregion

    }
}
