using System;

namespace MartianRobots
{
    public class ScentFoundException : Exception
    {
        public ScentFoundException()
            : base("Scent found")
        {

        }
    }
}