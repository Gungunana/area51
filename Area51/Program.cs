using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorForBaseArea51
{
    class Program
    {
        static void Main(string[] args)
        {
            var elevator = new Elevator(0, 4);
            var agents = new List<Agent>();

            for (int i = 0; i < 10; i++)
            {
                var agent = new Agent(i.ToString(), elevator);
                agents.Add(agent);
            }

            var threads = new List<Thread>();

            foreach (var ag in agents)
            {
                var thread = new Thread(ag.GoToFloor);
                thread.Start();
                threads.Add(thread);
            }

           threads.ForEach(x => x.Join());

            Console.WriteLine("Simulation completed.");
            Console.ReadLine();
        }
    }
}