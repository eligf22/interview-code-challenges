using System;

namespace MartianRobots.Exceptions
{
    public class ScentFoundException : Exception
    {
        public ScentFoundException()
            : base("Scent found")
        {

        }
    }
}