using System;
using System.Linq;
using System.Collections.Generic;

namespace MartianRobots.Models
{
    public class Scent
    {
        public Dictionary<string, string> Scents {get; set;}

        public Scent()
        {
            Scents = new Dictionary<string, string>();
        }
    }
}