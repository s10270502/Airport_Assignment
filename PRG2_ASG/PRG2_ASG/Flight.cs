using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASG
{
    abstract class Flight
    {
        public string flightNumber { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public DateTime expectedTime { get; set; }
        public string status { get; set; }
        public Flight() { }
        public Flight(string FN, string Origin, string Destination, DateTime ExpectedTime, string Status)
        {
            flightNumber = FN;
            origin = Origin;
            destination = Destination;
            expectedTime = ExpectedTime;
            status = Status;
        }
        public override string ToString()
        {
            return $" ";
        }

        public virtual double CalculateFees()
        {
          
        }




    }


}
