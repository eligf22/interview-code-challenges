using System;
using System.Linq;
using System.Collections.Generic;

namespace MartianRobots
{
    public class InputValidator
    {
       public static bool IsValid(string input)
       {
           if(string.IsNullOrEmpty(input))
           {
               Console.WriteLine("Input string cannot be null or empty");
               throw new ArgumentNullException("Invalid input.");
           }
           
           var arr = input.Split(Environment.NewLine.ToCharArray());
           if(arr.Length < 3)
           {
               Console.WriteLine("Input must contain at least the upper-right coordinates of the rectangular world and a Robot position and its instructions.");
               throw new ArgumentException("Invalid input.");
           }

           IsWorldValid(arr[0]);
           
           var robotList = arr.ToList();
           robotList.RemoveAt(0);
           
           if(robotList.Count % 2 != 0)
            {
                Console.WriteLine("Invalid input. Robots structure is not complete.");
                throw new ArgumentException("Invalid input structure.");
            }
            
           return true;
       }

       private static void IsWorldValid(string world)
       {
           var arrWorld = world.Split(' ');

           if(arrWorld.Length < 2)
           {
               Console.WriteLine("The rectangular world must have two coordinates: x and y");
               throw new ArgumentException("Rectangular world is invalid.");
           }

           var coordinateX= arrWorld[0].ToString();
           if(Int16.Parse(coordinateX) > 50)
           {
               Console.WriteLine("Coordinate X cannot be greater than 50.");
               throw new ArgumentException("Invalid Coordinate.");
           }

           var coordinateY= arrWorld[1].ToString();
           if(Int16.Parse(coordinateY) > 50)
           {
               Console.WriteLine("Coordinate Y cannot be greater than 50.");
               throw new ArgumentException("Invalid Coordinate.");
           }
       }
    }
}
