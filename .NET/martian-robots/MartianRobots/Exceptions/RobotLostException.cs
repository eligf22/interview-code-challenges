using System;

namespace MartianRobots.Exceptions
{
    public class RobotLostException : Exception
    {
        public RobotLostException()
            : base("Robot lost!")
        {

        }
    }
}