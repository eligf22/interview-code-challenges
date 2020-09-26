using System;

namespace MartianRobots
{
    public class RobotLostException : Exception
    {
        public RobotLostException()
            : base("Robot lost!")
        {

        }
    }
}