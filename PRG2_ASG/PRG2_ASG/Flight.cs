using System;
using System.Collections.Generic;
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
    abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }
        public Flight() { }
        public Flight(string FN, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = FN;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }
        public string GetFormattedExpected()
        {
            return ExpectedTime.ToString("d/M/yyyy h:mm:ss tt");
        }
        public override string ToString()
        {
            return $"Flight Number: {FlightNumber}\nOrigin: {Origin}\nDestination: {Destination}\nExpected Time:{GetFormattedExpected()}\nStatus: {Status}\n";
        }

        // abstract function, no implementation for calculate fees
        public virtual double CalculateFees(){
            return 0.0;
        }
    }


}
