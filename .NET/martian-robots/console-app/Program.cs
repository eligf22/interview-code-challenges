using System;
using System.Collections.Generic;
using MartianRobots;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace console_app
{
    public class Program
    {
        public static IMartianRobotsHandler MartianRobots { get; set; }
        static void Main(string[] args)
        {    
            var collection = new ServiceCollection();  
            collection.AddScoped <IMartianRobotsHandler, MartianRobotsHandler>(); 
            collection.AddScoped <IConsoleWriter, ConsoleWriter>();  
            IServiceProvider serviceProvider = collection.BuildServiceProvider();  
            MartianRobots = serviceProvider.GetService<IMartianRobotsHandler>();    

            //All robots location updated. One robot lost, one robot scent found.
            //var input = "5 3\n1 1 E\nRFRFRFRF\n3 2 N\nFRRFLLFFRRFLL\n3 1 N\nFFFL";

            //All robots location updated. One robot lost, one scent found.
            var input = "5 3\n1 1 E\nRFRFRFRF\n3 2 N\nFRRFLLFFRRFLL\n0 3 W\nLLFFFLFLFL";

            //All robots location updated. No robots lost.
            //var input = "5 5\n1 2 N\nLFLFLFLFF\n3 3 E\nFFRFFRFRRF";
            
            MartianRobots.Execute(input); 

            if (MartianRobots is IDisposable) {  
                ((IDisposable) MartianRobots).Dispose();  
            }          
        }

    }
}
