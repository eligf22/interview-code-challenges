using System;

namespace MartianRobots.Models
{
    public class Planet
    {
        public int CoordenateX {get; set;}

        public int CoordenateY { get; set; }

        public Planet(int x, int y)
        {
            CoordenateX = x;
            CoordenateY = y;
        }

    }
}
