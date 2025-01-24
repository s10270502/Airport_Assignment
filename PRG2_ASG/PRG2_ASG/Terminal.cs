using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASG
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

        public Terminal()
        {

        }

        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }

        public bool AddAirline(Airline airline)
        {
            if(Airlines.ContainsKey(airline.Code))
            {
                return false;
            }
            Airlines.Add(airline.Code, airline);
            return true;
        }

        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            BoardingGates.Add(boardingGate.GateName, boardingGate);
            return true;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach(var airline in Airlines.Values)
            {
                if (flight.FlightNumber.StartsWith(airline.Code))
                {
                    return airline;
                }
            }
            return null;
        }

        public void PrintAirlineFees()
        {
            foreach(var fee in Airlines.Values)
            {
                Console.WriteLine($"{fee.Name} Total Fee: {fee.CalculateFees}");
            }
        }

        public override string ToString()
        {
            return "Terminal Name: " + TerminalName;
        }
    }
}
