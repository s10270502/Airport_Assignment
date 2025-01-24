using PRG2_ASG;
using System;
using System.Collections.Generic;

// Feature 1 Load files (airlines and boarding gates) - Pierre
void LoadAirlinesAndBoardingGates(Terminal terminal)
{
    Console.WriteLine("Loading Airlines...");
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        string? s = sr.ReadLine(); // read the heading, to skip, not required heading. "Airline Name,Airline Code"
        // s = every line through the loop, "Singapore Airlines,SQ" 
        while ((s = sr.ReadLine()) != null)
        {
            // during each loop, process each line data
            string[] airlineData = s.Split(','); // ["Singapore Airlines","SQ" ]
            terminal.AddAirline(new Airline(airlineData[0], airlineData[1]));
        }
    }

    //Airline sqAirline = airlineDict["SQ"];
    //sqAirline.
    Console.WriteLine($"{terminal.Airlines.Count} Airlines Loaded!");
    Console.WriteLine("Loading Boarding Gates...");
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        string? s = sr.ReadLine(); // read the heading // Boarding Gate,DDJB,CFFT,LWTT -> Header

        while ((s = sr.ReadLine()) != null)
        {
            // A1,False,False,True
            string[] boardingData = s.Split(','); // ["A1", "False", "False", "True"]
            terminal.AddBoardingGate(new BoardingGate(boardingData[0], Boolean.Parse(boardingData[1]), Boolean.Parse(boardingData[2]), Boolean.Parse(boardingData[3]), null));
        }
    }
    Console.WriteLine($"{terminal.BoardingGates.Count} Boarding Gates Loaded!");
}

// Feature 4 - All Boarding Gates - Pierre
void ListBoardingGates(Terminal terminal)
{
    // 
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}LWTT");
    foreach(KeyValuePair<string, BoardingGate> data in terminal.BoardingGates)
    {
        BoardingGate boardingGate = data.Value;
        Console.WriteLine($"{boardingGate.GateName,-16}{boardingGate.SupportsDDJB,-23}{boardingGate.SupportsCFFT,-23}{boardingGate.SupportsLWTT}");
    }
}


// Feature 7 - Display full flight details from an airline - Pierre

//	list all the Airlines available
//	prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
//	retrieve the Airline object selected
//	for each Flight from that Airline, show their Airline Number, Origin and Destination
//	prompt the user to select a Flight Number
//	retrieve the Flight object selected
//	display the following Flight details, which are all the flight specifications (i.e. Flight Number, Airline Name, Origin, Destination, and Expected Departure/Arrival Time, Special Request Code (if any) and Boarding Gate (if any))


void ListAirline(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-16}Airline Name");
    foreach (KeyValuePair<string, Airline> data in terminal.Airlines)
    {
        Airline airline = data.Value;
        Console.WriteLine($"{airline.Code,-16}{airline.Name}");
    }

}
void DisplayFullFlightOfAirline(Terminal terminal)
{
    ListAirline(terminal);
    Console.Write("Enter Airline Code: ");
    string? airlineCode = Console.ReadLine().ToUpper(); // sq -> SQ
    if(airlineCode == null || !terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid airline code!"); // Validations
        return;// dont print anything
    }
    Airline selectedAirline = terminal.Airlines[airlineCode];
    List<Flight> relatedFlights = new List<Flight>();
    foreach(KeyValuePair<string, Flight> data in terminal.Flights) 
    {
        // Display matching airline code.
        // "SQ 115".Split(' ')[0] => ["SQ", "115"][0] => "SQ"
        string flightAirlineCode = data.Key.Split(' ')[0];
        if(flightAirlineCode == airlineCode)
        {
            relatedFlights.Add(data.Value);
        }
    }
    if (relatedFlights.Count > 0)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for Singapore Airlines");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected"}");
        foreach (Flight f in relatedFlights)
        {
            Console.WriteLine($"{f.FlightNumber,-16}{selectedAirline.Name,-23}{f.Origin,-23}{f.Destination,-23}{f.GetFormattedExpected()}");
        }

        Console.Write("Enter Flight Number: ");
        string? fn = Console.ReadLine().ToUpper(); // sq 115 -> SQ 115
        if (fn == null || !terminal.Flights.ContainsKey(fn))
        {
            Console.WriteLine("Invalid flight number!"); // Validations
            return;// dont print anything
        }

        Flight flight = terminal.Flights[fn];
        BoardingGate? boardingGate = null;
        foreach (KeyValuePair<string, BoardingGate> data in terminal.BoardingGates)
        {
            BoardingGate b = data.Value;
            if (b.Flight != null && b.Flight.FlightNumber == flight.FlightNumber)
            {
                boardingGate = b;
                break;
            }
        }
        Console.WriteLine(flight);
        Console.WriteLine(boardingGate);
    }
    else
    {
        Console.WriteLine($"Current airline {airlineCode} does not have any flights.");
    }
}


Terminal terminal5 = new Terminal("Terminal 5");

LoadAirlinesAndBoardingGates(terminal5);


string filePath = "flights.csv"; // Update with the correct file path
Console.WriteLine("Loading Flights...");
// Read the CSV file and create Flight objects
string[] lines = File.ReadAllLines(filePath);
for (int i = 1; i < lines.Length; i++) // Skip the header row
{
    string[] data = lines[i].Split(',');
    string fn = data[0];
    string origin = data[1];
    string destination = data[2];
    DateTime exepectedTime = Convert.ToDateTime(data[3]);
    string flightType = data[4];

    if (flightType == "DDJB")
    {
        terminal5.Flights.Add(fn, new DDJBFlight(fn, origin, destination, exepectedTime, "", 300));
    }
    else if (flightType == "LWTT")
    {
        terminal5.Flights.Add(fn, new LWTTFlight(fn, origin, destination, exepectedTime, "", 500));
    }
    else if (flightType == "CFFT")
    {
        terminal5.Flights.Add(fn, new CFFTFlight(fn, origin, destination, exepectedTime, "", 150));
    }
    else
    {
        terminal5.Flights.Add(fn, new NORMFlight(fn, origin, destination, exepectedTime, ""));
    }

}

Console.WriteLine($"{terminal5.Flights.Count} Flights Loaded!\n\n\n\n");
// program loop
while (true)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Please select your option:");

    string option = Console.ReadLine();
    if(option == "0")
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else if (option == "1")
    {

    }
    else if (option == "2")
    {
        ListBoardingGates(terminal5);
    }
    else if (option == "3")
    {

    }
    else if (option == "4")
    {

    }
    else if (option == "5")
    {
        DisplayFullFlightOfAirline(terminal5);
    }
    else if (option == "6")
    {

    }
    else if (option == "7")
    {

    }
    else
    {
        Console.WriteLine("Invalid Choice!");
    }

}
