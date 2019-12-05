using Area51;
using System;
using System.Threading;

namespace ElevatorForBaseArea51
{
    public class Elevator
    {
        private ElevatorStatus elevatorStatus;
        public int CurrentFloor { get; set; }
        public Semaphore Semaphore { get; set; }
        public int AllFloors { get; }
        public bool CanOpen { get; set; }

        public Elevator(int currentFloor, int allFloors)
        {
            elevatorStatus = ElevatorStatus.Stopped;
            Semaphore = new Semaphore(1, 1);
            CurrentFloor = currentFloor;
            AllFloors = allFloors;
        }

        public void Elevate(int aimFloor, int currentAgentFloor, AgentRole role)
        {
            switch (elevatorStatus)
            {
                case ElevatorStatus.Down:
                    GoDown(aimFloor, currentAgentFloor, role);
                    break;
                case ElevatorStatus.Up:
                    GoUp(aimFloor, currentAgentFloor, role);
                    break;
                case ElevatorStatus.Stopped:
                    if (CurrentFloor < currentAgentFloor)
                        GoUp(aimFloor, currentAgentFloor, role);
                    else if (CurrentFloor > currentAgentFloor)
                        GoDown(aimFloor, currentAgentFloor, role);
                    else
                        Stop(aimFloor, currentAgentFloor, role);
                    break;
            }
        }

        public void Stop(int aimFloor, int currentAgentFloor, AgentRole role)
        {
            elevatorStatus = ElevatorStatus.Stopped;
            CurrentFloor = currentAgentFloor;

            if (currentAgentFloor != aimFloor)
            {
                currentAgentFloor = aimFloor;
                Elevate(aimFloor, currentAgentFloor, role);
                return;
            }

            if (CanGetOffElevator(aimFloor, role))
            {
                CanOpen = true;
            }
            else
            {
                Console.WriteLine($"Agent with {role} role can't get off on this floor - {aimFloor + 1}");
                CanOpen = false;
            }

        }

        public void GoDown(int aimFloor, int currentAgentFloor, AgentRole role)
        {
            elevatorStatus = ElevatorStatus.Down;
            for (int i = CurrentFloor; i >= 0; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"The Elevator is on {i + 1} floor.");
                if (currentAgentFloor == i)
                {
                    Stop(aimFloor, i, role);
                    break;
                }
                else
                    continue;
            }
        }

        public void GoUp(int aimFloor, int currentAgentFloor, AgentRole role)
        {
            elevatorStatus = ElevatorStatus.Up;
            for (int i = CurrentFloor; i < AllFloors; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"The Elevator is on {i + 1} floor.");
                if (currentAgentFloor == i)
                {
                    Stop(aimFloor, i, role);
                    break;
                }
                else
                    continue;
            }
        }

        public bool CanGetOffElevator(int chosenFloor, AgentRole role)
        {
            if (role == AgentRole.Confidential && chosenFloor > 0)
                return false;
            else if (role == AgentRole.Secret && chosenFloor >= 2)
                return false;
            else
                return true;
        }
    }
}