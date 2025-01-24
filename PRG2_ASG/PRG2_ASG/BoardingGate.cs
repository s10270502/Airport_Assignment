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
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate()
        {
            
        }

        public BoardingGate(string gateName,  bool supportsDDJB, bool supportsCFFT, bool supportsLWTT, Flight? flight)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
        }

        public double CalculateFees()
        {
            return 300.0;
        }

        public override string ToString()
        {
            return "Gate Name: " + GateName +
                "\t Supports CFFT: " + SupportsCFFT +
                "\t Supports DDJB: " + SupportsDDJB +
                "\t Supports LWTT: " + SupportsLWTT;
        }
    }
}
