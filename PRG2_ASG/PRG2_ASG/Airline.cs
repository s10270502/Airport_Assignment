using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10270502F
// Student Name : Pierre
// Partner Name : Javier
//==========================================================

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

        public double CalculateDiscount()
        {
            double totalFees = CalculateFees();
            double discountGiven = 0.0;
            int numberOfFlights = Flights.Values.Count;
            int numberOfDiscount = numberOfFlights / 3;
            // For every 3 flights arriving/departing, airlines will receive a discount
            discountGiven = 350 * numberOfDiscount;
            // For each airline with more than 5 flights arriving/departing, the airline will receive an additional discount
            if (numberOfFlights > 5)
            {
                discountGiven += totalFees * 0.03;
            }

            foreach (var flight in Flights.Values)
            {

                // For each flight arriving/ departing before 11am or after 9pm
                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                {
                    discountGiven += 110;
                }

                //For each flight with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                {
                    discountGiven += 25;
                }

                // For each flight not indicating any Special Request Codes
                if (flight is NORMFlight)
                {
                    discountGiven += 50;
                }
            }
            return discountGiven;
        }

        public double CalculateFees()
        {
            double fees = 0.0;

            // Subtotal for all flight
            foreach (var flight in Flights.Values)
            {
                fees += flight.CalculateFees();
            }

            return fees;
        }

        public bool RemoveFlight(Flight flight)
        {
            Flights.Remove(flight.FlightNumber);
            return true;
        }

        public override string ToString()
        {
            return "Name " + Name +
                "\t Code " + Code;
        }
    }
}
