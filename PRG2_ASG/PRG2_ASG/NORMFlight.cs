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
    class NORMFlight : Flight
    {
        public NORMFlight(string FN, string Origin, string Destination, DateTime ExpectedTime, string Status) : base(FN, Origin, Destination, ExpectedTime, Status) { }
        public override string ToString()
        {
            return $" ";
        }

        public override double CalculateFees()
        {
            return base.CalculateFees();
        }
    }
}
