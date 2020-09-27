using System;

namespace MartianRobots.Models
{
    public class Position
    {
        public int CoordenateX {get; set;}

        public int CoordenateY { get; set; }

        public string Orientation { get; set; }

        public Position(int x, int y, string orientation)
        {
            CoordenateX = x;
            CoordenateY = y;
            Orientation = orientation;
        }
    }
}
