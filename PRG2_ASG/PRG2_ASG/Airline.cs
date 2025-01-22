using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASG
{
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        public Airline()
        {

        }

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public bool AddFlight(Flight flight)
        {
            if(Flights.ContainsKey(flight.flightNumber))
            {
                return false;
            }
            Flights.Add(Code, flight);
            return true;
        }

        public double CalculateFees()
        {
            int numberOfFlights = Flights.Values.Count;
            int numberOfDiscount = numberOfFlights / 3;
            int discountGiven = 350 * numberOfDiscount;
            if (numberOfFlights > 5)
            {
                
            }
        } 
    }
}
