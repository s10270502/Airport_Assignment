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
    abstract class Flight : IComparable
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }
        public string SpecialRequestCode { get; set; }
        public Flight() { }
        public Flight(string specialRequestCode, string FN, string origin, string destination, DateTime expectedTime, string status)
        {
            SpecialRequestCode = specialRequestCode;
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

        // virtual function
        public virtual double CalculateFees() {
            /*
             check if the Origin or Destination is Singapore (SIN), and apply the respective fee of $800 or $500 accordingly
             check if the Flight has indicated a Special Request Code and charge the appropriately listed Additional Fee (Auto polymorph)
             apply the Boarding Gate Base Fee of $300
             **/
            double baseBoardingGate = 300.0;
            if (Origin == "Singapore (SIN)")
            {
                return baseBoardingGate + 800.0;
            }
            else if (Destination == "Singapore (SIN)")
            {
                return baseBoardingGate + 500.0;
            }
            return baseBoardingGate;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null) return -1;

            Flight? otherFlight = obj as Flight;
            if (otherFlight != null)
                return this.ExpectedTime.CompareTo(otherFlight.ExpectedTime);
            else
                throw new ArgumentException("Object is not a Temperature");
        }
    }
    }



