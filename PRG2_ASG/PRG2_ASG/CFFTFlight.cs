using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASG
{
    class CFFTFlight : Flight
    {
        public double requestFee { get; set; }
        public CFFTFlight(string FN, string Origin, string Destination, DateTime ExpectedTime, string Status, double RequestFee) : base(FN, Origin, Destination, ExpectedTime, Status)
        {
            requestFee = RequestFee;
        }

        public override string ToString()
        {
            return $" ";
        }

        public override double CalculateFees()
        {
            return 0.0;
        }
    }
}
