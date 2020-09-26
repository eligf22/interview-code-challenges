using System;

namespace MartianRobots
{
    public class Robot
    {
        public Position RobotPosition { get; set; }

        public string Instruction { get; set; }

        public Robot(Position position, string instruction)
        {
            RobotPosition = position;
            Instruction = instruction;
        }
    }
}
