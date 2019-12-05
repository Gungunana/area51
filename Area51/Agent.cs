using Area51;
using System;

namespace ElevatorForBaseArea51
{
    public class Agent
    {
        private const int CountOfFloors = 4;

        public AgentRole AgentRole { get; set; }
        public string Name { get; set; }
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
        public Elevator Elevator { get; set; }

        public object myLock = new object();

        public Agent(string name, Elevator elevator)
        {
            GetAgentRole(Enum.GetValues(typeof(AgentRole)).Length);
            Name = name;
            Elevator = elevator;
        }

        public void GetAgentRole(int length)
        {
            int roleId = GetRandomValues(length);

            AgentRole = (AgentRole)roleId;
        }

        public void GoToFloor()
        {
            Elevator.Semaphore.WaitOne();

            Console.WriteLine($"Agent {Name} with role {AgentRole} is on the elevator");

            do
            {
                ChooseFloor();

                Elevator.Elevate(DestinationFloor, CurrentFloor, AgentRole);
                CurrentFloor = Elevator.CurrentFloor;
            }
            while (!Elevator.CanOpen);

            Console.WriteLine($"Agent {Name} with role {AgentRole} reached the {DestinationFloor + 1} floor.");
            Console.WriteLine(new string('X', 20));
            Console.WriteLine();

            Elevator.Semaphore.Release();
        }

        public void ChooseFloor()
        {
            DestinationFloor = GetRandomValues(CountOfFloors);
            if (AgentRole == AgentRole.TopSecret)
            {
                CurrentFloor = GetRandomValues(CountOfFloors);
            }
        }

        public int GetRandomValues(int length)
        {
            lock (myLock)
            {
                return new Random().Next(length);
            }
        }
    }
}
