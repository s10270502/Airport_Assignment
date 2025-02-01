using PRG2_ASG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

//==========================================================
// Student Number : S10270502F
// Student Name : Pierre
// Partner Name : Javier
//==========================================================

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

//Feature 2 - Load Flights (flights) - Javier
void LoadFlights(Terminal terminal)
{
    Dictionary<string, Flight> Flights = new Dictionary<string, Flight>();

    string filePath = "flights.csv"; // Update with the correct file path
    Console.WriteLine("Loading Flights...");
    // Read the CSV file and create Flight objects
    string[] lines = File.ReadAllLines(filePath);
    for (int i = 1; i < lines.Length; i++) // Skip the header row
    {
        string[] data = lines[i].Split(',');
        string fn = data[0].Trim().Replace(" ", "").ToUpper(); ;
        string origin = data[1];
        string destination = data[2];
        DateTime expectedTime = Convert.ToDateTime(data[3]);
        string flightType = data[4].Trim();

        if (flightType == "DDJB")
        {
            terminal.Flights.Add(fn, new DDJBFlight(flightType, fn, origin, destination, expectedTime, "", 300));
        }
        else if (flightType == "LWTT")
        {
            terminal.Flights.Add(fn, new LWTTFlight(flightType, fn, origin, destination, expectedTime, "", 500));
        }
        else if (flightType == "CFFT")
        {
            terminal.Flights.Add(fn, new CFFTFlight(flightType, fn, origin, destination, expectedTime, "", 150));
        }
        else
        {
            terminal.Flights.Add(fn, new NORMFlight(flightType, fn, origin, destination, expectedTime, ""));
        }

    }
}
// Feature 3 - List all flights with basic information - Javier
void DisplayFlights(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-23} {"Origin",-23} {"Destination",-23} {"Expected Departure/Arrival Time",-23}");

    foreach (KeyValuePair<string, Flight> data in terminal.Flights)
    {
        Flight flight = data.Value;

        // Get the airline associated with the flight
        Airline airline = terminal.GetAirlineFromFlight(flight);
        string airlineName = airline != null ? airline.Name : "Unknown";

        // Use the GetFormattedExpected method for a formatted date/time
        Console.WriteLine($"{flight.FlightNumber,-16} {airlineName,-23} {flight.Origin,-23} {flight.Destination,-23} {flight.GetFormattedExpected(),-23}");
    }
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
// Feature 5 - Assign a boarding gate to flight - Javier
static void DisplayFlightDetails(Flight flight)
{
    string specialCode = "None";
    if (flight is DDJBFlight)
    {
        specialCode = "DDJB";
    }
    else if (flight is CFFTFlight)
    {
        specialCode = "CFFT";
    }
    else if (flight is LWTTFlight)
    {
        specialCode = "LWTT";
    }

    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.GetFormattedExpected()}");
    Console.WriteLine($"Special Request Code: {specialCode}");
}

static void UpdateFlightStatus(Flight flight)
{
    if (flight == null)
    {
        Console.WriteLine("Unable to find flight. Cannot update status.");
        return;
    }

    Console.WriteLine("1. Delayed");
    Console.WriteLine("2. Boarding");
    Console.WriteLine("3. On Time");
    Console.WriteLine("Please select the new status of the flight:");

    string choice = Console.ReadLine().Trim();
    if (choice == "1")
    {
        flight.Status = "Delayed";
    }
    else if (choice == "2")
    {
        flight.Status = "Boarding";
    }
    else if (choice == "3")
    {
        flight.Status = "On Time";
    }
    else
    {
        Console.WriteLine("Invalid choice. Status remains unchanged.");
        return;
    }
}

static void AssignBoardingGate(Terminal terminal)
{

    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    Console.WriteLine("Enter Flight Number:");
    string flightNumber = Console.ReadLine()?.Trim().Replace(" ", "").ToUpper();

    if (string.IsNullOrEmpty(flightNumber))
    {
        Console.WriteLine("Flight number cannot be empty.");
        return;
    }

    // Extract airline code from flight number
    string airlineCode = "";
    if (!string.IsNullOrEmpty(flightNumber) && flightNumber.Length >= 2)
    {
        airlineCode = flightNumber.Substring(0, 2);
    }

    // Try to match flight number with or without airline code
    Flight flight = null;
    foreach (var f in terminal.Flights.Values)
    {
        string normalizedFlightNum = f.FlightNumber.Replace(" ", "").ToUpper();
        if (normalizedFlightNum == flightNumber ||
            (f.FlightNumber.Replace(" ", "").ToUpper().EndsWith(flightNumber) &&
             f.FlightNumber.ToUpper().StartsWith(airlineCode)))
        {
            flight = f;
            flightNumber = f.FlightNumber; // Use original flight number format
            break;
        }
    }

    if (flight == null)
    {
        Console.WriteLine("Flight not found.");
        return;
    }

    Console.WriteLine("Enter Boarding Gate Name:");
    string gateName = Console.ReadLine()?.Trim().Replace(" ", "").ToUpper();

    if (string.IsNullOrEmpty(gateName))
    {
        Console.WriteLine("Boarding gate name cannot be empty.");
        return;
    }

    if (!terminal.BoardingGates.TryGetValue(gateName, out BoardingGate gate))
    {
        Console.WriteLine("Gate not found.");
        return;
    }

    if (gate.Flight != null)
    {
        Console.WriteLine("Gate already assigned to another flight.");
        return;
    }

    // Display flight and gate details
    DisplayFlightDetails(flight);
    Console.WriteLine($"\nBoarding Gate: {gate.GateName}");

    // Update flight status
    Console.WriteLine("\nWould you like to update the status of the flight? (Y/N)");
    if (Console.ReadLine().Trim().ToUpper() == "Y")
    {
        UpdateFlightStatus(flight);
    }

    // Assign gate
    gate.Flight = flight;
    Console.WriteLine($"\nFlight {flightNumber} has been assigned to Boarding Gate {gateName}!");
}

// Feature 6 - Javier
string flightNumberPattern = @"^[a-zA-Z]{2}[0-9]{3}$";
string locationPattern = @"^[a-zA-Z\s]+\s[(]{1}[A-Z]{3}[)]{1}$";

void CreateFlight(Terminal terminal)
{
    string? flightNumber = String.Empty;
    string? requestCode = String.Empty;
    string? origin = String.Empty;
    string? destination = String.Empty;
    string? expectedTimeStr = String.Empty;

    Console.WriteLine("Enter Flight Number (AB 123):");
    flightNumber = Console.ReadLine()?.Trim().Replace(" ", "").ToUpper();

    if (flightNumber == null || flightNumber == String.Empty)
    {
        Console.WriteLine("Flight Number cannot be empty");
        return;
    }

    if (Regex.IsMatch(flightNumber, flightNumberPattern))
    {
        Console.WriteLine($"Flight Number: {flightNumber}");
    }
    else
    {
        Console.WriteLine($"{flightNumber} is in an invalid format");
        return;
    }

    Console.WriteLine("Enter Origin (ABCDEFG (ABC))");
    origin = Console.ReadLine()?.Trim();
    if (origin == null || origin == String.Empty)
    {
        Console.WriteLine("Origin cannot be empty");
        return;
    }
    if (Regex.IsMatch(origin, locationPattern))
    {
        Console.WriteLine($"Origin: {origin}");
    }
    else
    {
        Console.WriteLine($"{origin} is in an invalid format");
        return;
    }

    Console.WriteLine("Enter Destination (ABCDEFG (ABC))");
    destination = Console.ReadLine()?.Trim();
    if (destination == null || destination == String.Empty)
    {
        Console.WriteLine("Destination cannot be empty");
        return;
    }
    if (Regex.IsMatch(destination, locationPattern))
    {
        Console.WriteLine($"Destination: {destination}");
    }
    else
    {
        Console.WriteLine($"{destination} is in an invalid format");
        return;
    }

    Console.WriteLine("Enter Expected Time (e.g. 4.15 AM)");
    expectedTimeStr = Console.ReadLine()?.Trim();
    DateTime expectedTime;
    if (expectedTimeStr == null || expectedTimeStr == String.Empty)
    {
        Console.WriteLine("Expected Time cannot be empty");
        return;
    }
    try
    {
        expectedTime = DateTime.Parse(expectedTimeStr);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        return;
    }

    Console.WriteLine("Enter Special Request Code (leave empty if NA)");
    requestCode = Console.ReadLine()?.Trim().Replace(" ", "").ToUpper();

    Flight newFlight;
    if (requestCode == "DDJB")
    {
        newFlight = new DDJBFlight(requestCode, flightNumber, origin, destination, expectedTime, "Scheduled", 300);
    }
    else if (requestCode == "CFFT")
    {
        newFlight = new CFFTFlight(requestCode, flightNumber, origin, destination, expectedTime, "Scheduled", 150);
    }
    else if (requestCode == "LWTT")
    {
        newFlight = new LWTTFlight(requestCode, flightNumber, origin, destination, expectedTime, "Scheduled", 500);
    }
    else if (requestCode == "")
    {
        newFlight = new NORMFlight(requestCode, flightNumber, origin, destination, expectedTime, "Scheduled");
    }
    else
    {
        Console.WriteLine("Invalid Request Code");
        return;
    }

    terminal.Flights.Add(newFlight.FlightNumber, newFlight);

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
    // Might need to grab flights from Airline class, instead of terminal direcly.
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


// Initialisations
Terminal terminal5 = new Terminal("Terminal 5");
LoadAirlinesAndBoardingGates(terminal5);
LoadFlights(terminal5);
Console.WriteLine($"{terminal5.Flights.Count} Flights Loaded!\n\n\n\n");


// Feature 9 - Javier
ArrayList SortFlightsChronologically(Terminal terminal)
{
    ArrayList flightsArr = new ArrayList();
    foreach (Flight f in terminal.Flights.Values)
    {
        flightsArr.Add(f);
    }
    flightsArr.Sort();
    return flightsArr;
}

void DisplayFlightsArr(ArrayList flightsArr, Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-23} {"Origin",-23} {"Destination",-23} {"Expected Departure/Arrival Time",-36}{"Boarding Gate",-23}{"Special Request Code"}");

    foreach (Flight flight in flightsArr)
    {

        // Get the airline associated with the flight
        Airline airline = terminal.GetAirlineFromFlight(flight);
        string airlineName = airline != null ? airline.Name : "Unknown";
        string assignedGateName = "";
        foreach (BoardingGate b in terminal.BoardingGates.Values)
        {
            if (b.Flight == null)
            {
                continue;
            }
            if (b.Flight.FlightNumber == flight.FlightNumber)
            {
                BoardingGate assignedGate = b;
                assignedGateName = assignedGate.GateName;
            }
        }
        // Use the GetFormattedExpected method for a formatted date/time
        Console.WriteLine($"{flight.FlightNumber,-16} {airlineName,-23} {flight.Origin,-23} {flight.Destination,-23} {flight.GetFormattedExpected(),-36}{assignedGateName,-23}{flight.SpecialRequestCode}");
    }
}

// Advanced Feature A - Javier
void BulkProcessFlights(Terminal terminal)
{
    int unassignedCounter = 0;
    int assignedCounter = 0;
    bool assignedFlag = false;
    Queue<Flight> flightQueue = new Queue<Flight>();

    // enqueuing
    foreach (Flight f in terminal.Flights.Values)
    {
        foreach (BoardingGate b in terminal.BoardingGates.Values)
        {
            if (b.Flight == null)
            {
                continue;
            }
            if (f.FlightNumber == b.Flight.FlightNumber)
            {
                Console.WriteLine($"Flight {f.FlightNumber} is already assigned to Gate {b.GateName}");
                assignedFlag = true;
                assignedCounter += 1;
            }
        }
        if (assignedFlag == false)
        {
            unassignedCounter += 1;
            flightQueue.Enqueue(f);
            Console.WriteLine($"Enqueuing {f.FlightNumber} for processing");
        }
        assignedFlag = false; // reset 'true' assignedFlag
    }

    // counting unassigned gates
    int unassignedGateCounter = 0;
    foreach (BoardingGate b in terminal.BoardingGates.Values)
    {
        if (b.Flight == null)
        {
            // means it is unassigned, therefore,
            unassignedGateCounter += 1;
        }
    }

    // total number of unassigned flights and boarding gates
    Console.WriteLine($"\nThere are {unassignedCounter} flights that are not assigned a boarding gate");
    Console.WriteLine($"There are {unassignedGateCounter} boarding gates that are not assigned a flight");

    // begin processing

    int flightsProcessed = 0;
    int alreadyAssigned = assignedCounter;

    while (flightQueue.Count() > 0)
    {
        Flight target = flightQueue.Dequeue();
        Console.WriteLine($"Processing flight {target.FlightNumber}");
        string requestCode = target.SpecialRequestCode;
        BoardingGate? targetGate = null;

        // first pass of assignment (stricter, only looking for single support gates)
        if (requestCode != String.Empty)
        {
            Console.Write($"    Considering gates");
            if (requestCode == "LWTT")
            {
                foreach (BoardingGate b in terminal.BoardingGates.Values)
                {
                    Console.Write($" {b.GateName}");
                    if (b.SupportsLWTT && !b.SupportsDDJB && !b.SupportsCFFT && b.Flight == null)
                    {
                        targetGate = b;
                        break;
                    }
                }
            }
            else if (requestCode == "DDJB")
            {
                foreach (BoardingGate b in terminal.BoardingGates.Values)
                {
                    Console.Write($" {b.GateName}");
                    if (!b.SupportsLWTT && b.SupportsDDJB && !b.SupportsCFFT && b.Flight == null)
                    {
                        targetGate = b;
                        break;
                    }
                }
            }
            else if (requestCode == "CFFT")
            {
                foreach (BoardingGate b in terminal.BoardingGates.Values)
                {
                    Console.Write($" {b.GateName}");
                    if (!b.SupportsLWTT && !b.SupportsDDJB && b.SupportsCFFT && b.Flight == null)
                    {
                        targetGate = b;
                        break;
                    }
                }
            }
            Console.WriteLine("");
        }
        else
        { // no code
            Console.Write($"    Considering gates");
            foreach (BoardingGate b in terminal.BoardingGates.Values)
            {
                Console.Write($" {b.GateName}");
                if (!b.SupportsLWTT && !b.SupportsDDJB && !b.SupportsCFFT && b.Flight == null)
                {
                    targetGate = b;
                    break;
                }
            }
            Console.WriteLine("");
        }

        // second pass, more lenient
        if ((requestCode != String.Empty) && targetGate == null)
        {
            Console.Write($"    (Second pass) Reconsidering gates");
            if (requestCode == "LWTT")
            {
                foreach (BoardingGate b in terminal.BoardingGates.Values)
                {
                    Console.Write($" {b.GateName}");
                    if (b.SupportsLWTT && b.Flight == null)
                    {
                        targetGate = b;
                        break;
                    }
                }
            }
            else if (requestCode == "DDJB")
            {
                foreach (BoardingGate b in terminal.BoardingGates.Values)
                {
                    Console.Write($" {b.GateName}");
                    if (b.SupportsDDJB && b.Flight == null)
                    {
                        targetGate = b;
                        break;
                    }
                }
            }
            else if (requestCode == "CFFT")
            {
                foreach (BoardingGate b in terminal.BoardingGates.Values)
                {
                    Console.Write($" {b.GateName}");
                    if (b.SupportsCFFT && b.Flight == null)
                    {
                        targetGate = b;
                        break;
                    }
                }
            }
            Console.WriteLine("");
        }
        else if ((requestCode == String.Empty) && targetGate == null)
        {
            foreach (BoardingGate b in terminal.BoardingGates.Values)
            {
                Console.Write($" {b.GateName}");
                if (b.Flight == null)
                {
                    targetGate = b;
                    break;
                }
            }
            Console.WriteLine("");
        }

        if (targetGate == null)
        { // no matching gates found
            Console.WriteLine($"    No available gate found for flight {target.FlightNumber} with request code {target.SpecialRequestCode}");
            continue;
        }
        else
        {
            Console.WriteLine($"    Flight {target.FlightNumber} has been assigned to gate {targetGate.GateName}\n");
        }

        // assignment of flight to gate
        terminal.BoardingGates[targetGate.GateName].Flight = target;
        flightsProcessed += 1;


    }

    DisplayFlights(terminal);
    Console.WriteLine($"Total number of Flights and Boarding Gates processed and assigned: {flightsProcessed}");
    Console.WriteLine($"Total number of Flights and Boarding Gates previously processed: {alreadyAssigned}");
    if (alreadyAssigned == 0)
    {
        Console.WriteLine($"Percentage of Automatically Processed / Previously Processed: Cannot divide by zero");
        return;
    }
    double quotient = flightsProcessed / alreadyAssigned;
    Console.WriteLine($"Percentage of Automatically Processed / Previously Processed: {quotient * 100}%");
    return;
}

//Console.WriteLine($"{terminal5.Flights.Count} Flights Loaded!\n\n\n\n");
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
    Console.WriteLine("A. Bulk Process Unassigned Flights");
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
        Console.WriteLine("");
        DisplayFlights(terminal5);
    }
    else if (option == "2")
    {
        Console.WriteLine("");
        ListBoardingGates(terminal5);
    }
    else if (option == "3")
    {
        Console.WriteLine("");
        AssignBoardingGate(terminal5);
    }
    else if (option == "4")
    {

    }
    else if (option == "5")
    {
        Console.WriteLine("");
        DisplayFullFlightOfAirline(terminal5);
    }
    else if (option == "6")
    {

    }
    else if (option == "7")
    {
        Console.WriteLine("");
        DisplayFlightsArr(SortFlightsChronologically(terminal5), terminal5);
    }
    else if (option == "A" || option == "a")
    {
        Console.WriteLine("");
        BulkProcessFlights(terminal5);
    }
    else
    {
        Console.WriteLine("Invalid Choice!");
    }

}
