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
            if(Flights.ContainsKey(flight.FlightNumber))
            {
                return false;
            }
            Flights.Add(flight.FlightNumber, flight);
            return true;
        }

        public double CalculateFees()
        {
            double totalDiscount = 0;
            double totalFees = 0;   
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            int numberOfFlights = Flights.Values.Count;
            int numberOfDiscount = numberOfFlights / 3;
            int discountGiven = 350 * numberOfDiscount;
            if (numberOfFlights > 5)
            {
                
            }

            return 0.0;
        } 
    }
}
