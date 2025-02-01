﻿using System;
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
    class LWTTFlight : Flight
    {
        public double requestFee { get; set; }
        public LWTTFlight(string SpecialRequestCode, string FN, string Origin, string Destination, DateTime ExpectedTime, string Status, double RequestFee) : base(SpecialRequestCode, FN, Origin, Destination, ExpectedTime, Status)
        {
            requestFee = RequestFee;
        }

        public override string ToString()
        {
            return base.ToString() + $", LWTT";
        }

        public override double CalculateFees()
        {
            return base.CalculateFees() + requestFee;
        }
    }
}
