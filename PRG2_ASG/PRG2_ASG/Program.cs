﻿using PRG2_ASG;

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
{   // to store flight objects with flight number as key
    Dictionary<string, Flight> Flights = new Dictionary<string, Flight>();

    string filePath = "flights.csv"; // Update with the correct file path
    Console.WriteLine("Loading Flights...");
    // Read the CSV file and create Flight objects
    string[] lines = File.ReadAllLines(filePath);
    //loop thru each line and skip header
    for (int i = 1; i < lines.Length; i++) 
    {
        string[] data = lines[i].Split(',');
        //extract flight data
        string fn = data[0]; // flight number
        string origin = data[1]; // origin airport
        string destination = data[2]; //destination airport
        DateTime expectedTime = Convert.ToDateTime(data[3]); //flight time
        string flightType = data[4].Trim();

        if (flightType == "DDJB")
        {   //creating flight
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

    //loop thru each flight in terminal flight dict
    foreach (KeyValuePair<string, Flight> data in terminal.Flights)
    {
        Flight flight = data.Value;

        // geting the airline associated with the flight
        Airline airline = terminal.GetAirlineFromFlight(flight);
        // if airlinename found, use it if not display unknown
        string airlineName = airline != null ? airline.Name : "Unknown";

        // Use the GetFormattedExpected method for a formatted date/time
        Console.WriteLine($"{flight.FlightNumber,-16} {airlineName,-23} {flight.Origin,-23} {flight.Destination,-23} {flight.GetFormattedExpected(),-23}");
    }
}

// Feature 4 (Option 2) - All Boarding Gates - Pierre
void ListBoardingGates(Terminal terminal)
{
    // 
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}LWTT");
    foreach (KeyValuePair<string, BoardingGate> data in terminal.BoardingGates)
    {
        BoardingGate boardingGate = data.Value;
        Console.WriteLine($"{boardingGate.GateName,-16}{boardingGate.SupportsDDJB,-23}{boardingGate.SupportsCFFT,-23}{boardingGate.SupportsLWTT}");
    }
}
// Feature 5 - Assign a boarding gate to flight - Javier
static void DisplayFlightDetails(Flight flight)
{
    string specialCode = "None";
    //assign corresponding code
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
    else if (flight is NORMFlight)
    {
        specialCode = "None";
    }
    //display details in format
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
    //get user choice and update status accordingly
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

    // extract airline code from flight number
    string airlineCode = "";
    if (!string.IsNullOrEmpty(flightNumber) && flightNumber.Length >= 2)
    {
        airlineCode = flightNumber.Substring(0, 2);
    }

    // try to match flight number with or without airline code
    Flight flight = null;
    foreach (var f in terminal.Flights.Values)
    {
        string normalizedFlightNum = f.FlightNumber.Replace(" ", "").ToUpper();
        //match with either exact flight number of flight number withh airline code
        if (normalizedFlightNum == flightNumber ||
            (f.FlightNumber.Replace(" ", "").ToUpper().EndsWith(flightNumber) &&
             f.FlightNumber.ToUpper().StartsWith(airlineCode)))
        {
            flight = f;
            flightNumber = f.FlightNumber; // using original flight number format
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

    //check if gate exists and is available
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
    Console.WriteLine($"Boarding Gate: {gate.GateName}");

    if (gate.SupportsDDJB)
    {
        string supportsDDJB = "True";
        Console.WriteLine($"Supports DDJB: {supportsDDJB}");
    }
    else
    {
        string supportsDDJB = "False";
        Console.WriteLine($"Supports DDJB: {supportsDDJB}");
    }

    if (gate.SupportsCFFT)
    {
        string supportsCFFT = "True";
        Console.WriteLine($"Supports CFFT: {supportsCFFT}");
    }
    else
    {
        string supportsCFFT = "False";
        Console.WriteLine($"Supports CFFT: {supportsCFFT}");
    }

    if (gate.SupportsLWTT)
    {
        string supportsLWTT = "True";
        Console.WriteLine($"Supports LWTT: {supportsLWTT}");
    }
    else
    {
        string supportsLWTT = "False";
        Console.WriteLine($"Supports LWTT: {supportsLWTT}");
    }



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
string flightNumberPattern = @"^[a-zA-Z]{2}[0-9]{3}$"; // 2 letters followed by 3 digits
string locationPattern = @"^[a-zA-Z\s]+\s[(]{1}[A-Z]{3}[)]{1}$"; // city name followed by 3 letter code

void CreateFlight(Terminal terminal)
{
    //initialize empty strings for flight details
    string? flightNumber = String.Empty;
    string? requestCode = String.Empty;
    string? origin = String.Empty;
    string? destination = String.Empty;
    string? expectedTimeStr = String.Empty;

    //get flight number
    Console.WriteLine("Enter Flight Number: ");
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

    //get origin
    Console.WriteLine("Enter Origin: ");
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

    //get destination
    Console.WriteLine("Enter Destination: ");
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

    //get expected time
    Console.WriteLine("Enter Expected Time: ");
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

    //get request code
    Console.WriteLine("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
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
    else if (requestCode == "NORM")
    {
        newFlight = new NORMFlight(requestCode, flightNumber, origin, destination, expectedTime, "Scheduled");
    }
    else
    {
        Console.WriteLine("Invalid Request Code");
        return;
    }

    //add flight to terminal flight collection
    terminal.Flights.Add(newFlight.FlightNumber, newFlight);
    Console.WriteLine($"Flight {flightNumber} has been added!");

}
// Feature 7 (Option 5) - Display full flight details from an airline - Pierre

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

string GetFlightSpecialRequestCode(Flight flight)
{
    if (flight is LWTTFlight)
    {
        return "LWTT";
    }
    else if (flight is CFFTFlight)
    {
        return "CFFT";
    }
    else if (flight is DDJBFlight)
    {
        return "DDJB";
    }
    else
    {
        return "None";
    }
}
// Feature 7 & 8 (Option 5/6) - Display full flight and modify flight details - Pierre
void PrintFlightInformation(Terminal terminal, string fn, Airline selectedAirline)
{
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

    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {flight.GetFormattedExpected()}");
    Console.WriteLine($"Status: {flight.Status}");
    Console.WriteLine($"Special Request Code: {GetFlightSpecialRequestCode(flight)}");
    Console.WriteLine($"Boarding Gate: {((boardingGate == null) ? "Unassigned" : boardingGate.GateName)}");
    Console.WriteLine("\n\n\n");
    /*
        Flight Number: QF 456
        Airline Name: Qantas Airways
        Origin: Singapore (SIN)
        Destination: Dubai (DXB)
        Expected Departure/Arrival Time: 13/1/2025 4:30:00 pm
        Status: Scheduled
        Special Request Code: CFFT
        Boarding Gate: Unassigned
     */
}
void DisplayFullFlightOfAirline(Terminal terminal)
{
    ListAirline(terminal);
    Console.Write("Enter Airline Code: ");
    string? airlineCode = Console.ReadLine().ToUpper(); // sq -> SQ
    if (airlineCode == null || !terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid airline code!"); // Validations
        return;// dont print anything
    }
    Airline selectedAirline = terminal.Airlines[airlineCode];
    List<Flight> relatedFlights = new List<Flight>();
    // Might need to grab flights from Airline class, instead of terminal direcly.
    foreach (KeyValuePair<string, Flight> data in terminal.Flights)
    {
        // Display matching airline code.
        // "SQ 115".Split(' ')[0] => ["SQ", "115"][0] => "SQ"
        string flightAirlineCode = data.Key.Split(' ')[0];
        if (flightAirlineCode == airlineCode)
        {
            relatedFlights.Add(data.Value);
        }
    }
    if (relatedFlights.Count > 0)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time"}");
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
        PrintFlightInformation(terminal, fn, selectedAirline);

    }
    else
    {
        Console.WriteLine($"Current airline {airlineCode} does not have any flights.");
    }
}

// Feature 8 (Option 6) - Modify Flight Details - Pierre
static bool modifyFlightStatus(Flight flight)
{
    if (flight == null)
    {
        Console.WriteLine("Invalid flight. Cannot update status.");
        return false;
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
        return false;
    }
    return true;
}
void ModifyFlightDetails(Terminal terminal)
{
    ListAirline(terminal);
    Console.WriteLine("Enter Airline Code: ");
    string? airlineCode = Console.ReadLine().ToUpper(); // sq -> SQ
    if (airlineCode == null || !terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid airline code!"); // Validations
        return;// dont print anything
    }
    Airline selectedAirline = terminal.Airlines[airlineCode];
    List<Flight> relatedFlights = new List<Flight>();
    // Might need to grab flights from Airline class, instead of terminal direcly.
    foreach (KeyValuePair<string, Flight> data in terminal.Flights)
    {
        // Display matching airline code.
        // "SQ 115".Split(' ')[0] => ["SQ", "115"][0] => "SQ"
        string flightAirlineCode = data.Key.Split(' ')[0];
        if (flightAirlineCode == airlineCode)
        {
            relatedFlights.Add(data.Value);
        }
    }
    if (relatedFlights.Count > 0)
    {
        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time"}");
        foreach (Flight f in relatedFlights)
        {
            Console.WriteLine($"{f.FlightNumber,-16}{selectedAirline.Name,-23}{f.Origin,-23}{f.Destination,-23}{f.GetFormattedExpected()}");
        }

        Console.WriteLine("Choose an existing Flight to modify or delete: ");
        string? fn = Console.ReadLine().ToUpper(); // sq 115 -> SQ 115
        if (fn == null || !terminal.Flights.ContainsKey(fn))
        {
            Console.WriteLine("Invalid flight number!"); // Validations
            return;// dont print anything
        }

        Flight flight = terminal.Flights[fn]; // Technically and assume that flight object is shared with BoardingGate and Airline.
        Console.WriteLine($"1. Modify Flight");
        Console.WriteLine($"2. Delete Flight");
        Console.WriteLine($"Choose an option:");
        string editDeleteOption = Console.ReadLine();
        if (editDeleteOption == "1")
        {
            // Edit
            Console.WriteLine($"1.Modify Basic Information");
            Console.WriteLine($"2.Modify Status");
            Console.WriteLine($"3.Modify Special Request Code");
            Console.WriteLine($"4.Modify Boarding Gate");
            Console.WriteLine($"Choose an option:");
            string modifyOption = Console.ReadLine();
            if (modifyOption == "1")
            {
                Console.Write($"Enter new Origin: ");
                string newOrigin = Console.ReadLine();
                Console.Write($"Enter new Destination: ");
                string newDest = Console.ReadLine();
                Console.Write($"Enter new Expected Departure/ Arrival Time(dd/mm/yyyy hh:mm): "); // e.g 13/1/2025 16:30
                string newDateStr = Console.ReadLine();
                DateTime newDt;
                string format = "dd/MM/yyyy HH:mm";

                // Try to parse the input based on the format provided
                if (DateTime.TryParseExact(newDateStr, format, null, System.Globalization.DateTimeStyles.None, out newDt))
                {
                    flight.Origin = newOrigin;
                    flight.Destination = newDest;
                    flight.ExpectedTime = newDt;
                    Console.WriteLine($" " + flight);
                }
                else
                {
                    Console.WriteLine("Invalid date and time format. Please use dd/MM/yyyy HH:mm.");
                    return;
                }
                Console.WriteLine("Flight updated!");
                PrintFlightInformation(terminal, fn, selectedAirline);
            }
            else if (modifyOption == "2")
            {
                if (modifyFlightStatus(flight))
                {
                    Console.WriteLine("Flight updated!");
                    PrintFlightInformation(terminal, fn, selectedAirline);
                }
            }
            else if (modifyOption == "3")
            {
                Console.WriteLine("Change special request code: ");
                Console.WriteLine("1. DDJB");
                Console.WriteLine("2. CFFT");
                Console.WriteLine("3. LWTT");
                Console.WriteLine("4. Normal");
                Console.WriteLine($"Choose an option:");
                string requestCodeChange = Console.ReadLine();
                Flight newFlight = null;
                if (requestCodeChange == "1")
                {
                    newFlight = new DDJBFlight("DDJB", flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status, 300);
                }
                else if (requestCodeChange == "2")
                {
                    newFlight = new CFFTFlight("CFFT", flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status, 150);
                }
                else if (requestCodeChange == "3")
                {
                    newFlight = new LWTTFlight("LWTT", flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status, 500);
                }
                else if (requestCodeChange == "4")
                {
                    newFlight = new NORMFlight("", flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status); // Or "NORM" if you have a specific code
                }
                else
                {
                    Console.WriteLine("Invalid Special Request selection!");
                    return;
                }

                // Due to special request change, we need to reconstruct the flight class accordingly.
                // Creation of new Flight object, flight = newFlight, this will only update for terminal class. Airline & boarindg gate will not.
                // All affected Flights in Airline, terminal and boarding, needs to be updated.
                // Edge case: If new special request is change, then now the current boarding gate is not supporting the type, we will remove Flight from boarding gate,
                if (newFlight != null)
                {
                    terminal.Flights[fn] = newFlight;
                    selectedAirline.Flights[fn] = newFlight;

                    foreach (KeyValuePair<string, BoardingGate> data in terminal.BoardingGates)
                    {
                        if (data.Value.Flight != null && data.Value.Flight.FlightNumber == flight.FlightNumber)
                        {
                            if (requestCodeChange == "1" && terminal.BoardingGates[data.Key].SupportsDDJB)
                            {
                                terminal.BoardingGates[data.Key].Flight = newFlight;
                            }
                            else if (requestCodeChange == "2" && terminal.BoardingGates[data.Key].SupportsCFFT)
                            {
                                terminal.BoardingGates[data.Key].Flight = newFlight;
                            }
                            else if (requestCodeChange == "3" && terminal.BoardingGates[data.Key].SupportsLWTT)
                            {
                                terminal.BoardingGates[data.Key].Flight = newFlight;
                            }
                            else if (requestCodeChange == "4")
                            {
                                terminal.BoardingGates[data.Key].Flight = newFlight;
                            }
                            else
                            {
                                terminal.BoardingGates[data.Key].Flight = null;
                                Console.WriteLine($"Removing Flight from boarding gate {terminal.BoardingGates[data.Key].GateName} as gate is not supporting the new flight special request code.");
                            }

                        }
                    }
                }
                Console.WriteLine("Flight updated!");
                PrintFlightInformation(terminal, fn, selectedAirline);

            }
            else if (modifyOption == "4")
            {
                ListBoardingGates(terminal);
                Console.WriteLine("Enter Boarding Gate Name to change/assign gate:");
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
                bool canSwapGate = true;
                if (flight is DDJBFlight && !gate.SupportsDDJB)
                {
                    canSwapGate = false;
                }
                else if (flight is CFFTFlight && !gate.SupportsCFFT)
                {
                    canSwapGate = false;
                }
                else if (flight is LWTTFlight && !gate.SupportsLWTT)
                {
                    canSwapGate = false;
                }

                if (canSwapGate == false)
                {
                    Console.WriteLine("Selected Gate does not support this flight special request code.");
                    return;
                }
                // Remove flight from the baording gate that its currrently occupying.
                foreach (KeyValuePair<string, BoardingGate> data in terminal.BoardingGates)
                {
                    if (data.Value.Flight != null && data.Value.Flight.FlightNumber == flight.FlightNumber)
                    {
                        terminal.BoardingGates[data.Key].Flight = null;
                    }
                }
                // Add flight into the new boarding gate.
                gate.Flight = flight;
                Console.WriteLine("Flight updated!");
                PrintFlightInformation(terminal, fn, selectedAirline);
            }
            else
            {
                Console.WriteLine("Invalid modify option selected!"); // Validations
                return;// dont print anything
            }

        }
        else if (editDeleteOption == "2")
        {
            // Delete from Airline and Terminal and Boarding Gate
            Console.WriteLine($"Confirm to delete flight(y/n):");
            string isDeleteFLight = Console.ReadLine().ToUpper();
            if (isDeleteFLight == "Y")
            {
                terminal.Flights.Remove(fn);
                selectedAirline.RemoveFlight(flight);

                foreach (KeyValuePair<string, BoardingGate> data in terminal.BoardingGates)
                {
                    if (data.Value.Flight != null && data.Value.Flight.FlightNumber == flight.FlightNumber)
                    {
                        terminal.BoardingGates[data.Key].Flight = null;
                    }
                }
                Console.WriteLine($"Remove Flight {flight.FlightNumber} successfully.");
            }

        }
        else
        {

            Console.WriteLine("Invalid edit/delete option selected!"); // Validations
            return;// dont print anything
        }

    }
    else
    {
        Console.WriteLine($"Current airline {airlineCode} does not have any flights.");
    }
}

// Advanced Feature B (Option B) - Pierre
/*
 (b)	Display the total fee per airline for the day
	check that all Flights have been assigned Boarding Gates; if there are Flights that have not been assigned, display a message for the user to ensure that all unassigned Flights have their Boarding Gates assigned before running this feature again
	for each Airline, retrieve all their Flights
o	for each Flight
	check if the Origin or Destination is Singapore (SIN), and apply the respective fee of $800 or $500 accordingly
	check if the Flight has indicated a Special Request Code and charge the appropriately listed Additional Fee
	apply the Boarding Gate Base Fee of $300
	compute the subtotal of fees to be charged for each Airline for the day
	compute the subtotal of discounts to be applied for each Airline based on the Promotional Conditions that they qualify for
	display the total final fees to be charged with a breakdown of the original subtotal calculated against the subtotal of discounts for the day
	compute and display the subtotal of all the Airline fees to be charged, the subtotal of all Airline discounts to be deducted, 
the final total of Airline fees that Terminal 5 will collect, and the percentage of the subtotal discounts over the final total of fees
 */
void DisplayTotalFeePerAirLine(Terminal terminal)
{
    foreach (KeyValuePair<string, Flight> flightData in terminal.Flights)
    {
        Flight flight = flightData.Value;
        bool flightHasBoardingGate = false;
        foreach (KeyValuePair<string, BoardingGate> data in terminal.BoardingGates)
        {
            if (data.Value.Flight != null && data.Value.Flight.FlightNumber == flight.FlightNumber)
            {
                flightHasBoardingGate = true;
            }
        }
        if (flightHasBoardingGate == false)
        {
            // check that all Flights have been assigned Boarding Gates; if there are Flights that have not been assigned,
            // display a message for the user to ensure that all unassigned Flights have their Boarding
            Console.WriteLine("Not all flights are assigned to a boarding gate");
            return;
        }
    }
    double totalT5Fees = 0.0;
    double totalT5Discounts = 0.0;
    foreach (KeyValuePair<string, Airline> airline in terminal.Airlines)
    {
        Console.WriteLine($"Fees Report for airline {airline.Value.Name}");
        double airlineFee = airline.Value.CalculateFees();
        double airlineDiscount = airline.Value.CalculateDiscount();
        // :C is for currency
        Console.WriteLine($"Airline Fee: {airlineFee:C}");
        Console.WriteLine($"Airline Discount: {airlineDiscount:C}");
        Console.WriteLine($"Airline SubTotal: {airlineFee - airlineDiscount:C}");
        totalT5Fees += airlineFee;
        totalT5Discounts += airlineDiscount;
    }
    // compute and display the subtotal of all the Airline fees to be charged, the subtotal of all Airline discounts to be deducted, 
    // the final total of Airline fees that Terminal 5 will collect, and the percentage of the subtotal discounts over the final total of fees
    Console.WriteLine($"Total Airline Fees Report");
    Console.WriteLine($"Total Fee: {totalT5Fees:C}");
    Console.WriteLine($"Total Discount: {totalT5Discounts:C}");
    double subTotal = totalT5Fees - totalT5Discounts;
    Console.WriteLine($"Sub Total Fee: {subTotal:C}");
    Console.WriteLine($"Discount %: {totalT5Discounts / subTotal:f2}%");
}


// Initialisations
Terminal terminal5 = new Terminal("Terminal 5");
LoadAirlinesAndBoardingGates(terminal5);
LoadFlights(terminal5);
Console.WriteLine($"{terminal5.Flights.Count} Flights Loaded!\n\n\n\n");


// Feature 9 - Javier
ArrayList SortFlightsChronologically(Terminal terminal)
{   // new array list
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
    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-23} {"Origin",-23} {"Destination",-23} {"Expected Departure/Arrival Time",-36}{"Status", -23}{"Boarding Gate",-23}{"Special Request Code"}");

    foreach (Flight flight in flightsArr)
    {

        // get the airline associated with the flight
        Airline airline = terminal.GetAirlineFromFlight(flight);
        string airlineName = airline != null ? airline.Name : "Unknown";
        string assignedGateName = "";
        //search thru all boarding gate to match to flight
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
        
        Console.WriteLine($"{flight.FlightNumber,-16} {airlineName,-23} {flight.Origin,-23} {flight.Destination,-23} {flight.GetFormattedExpected(),-36}{flight.Status, -27}{assignedGateName,-23}{flight.SpecialRequestCode}");
    }
}

// Advanced Feature A - Javier
void BulkProcessFlights(Terminal terminal)
{
    //initialize 
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
        //if flight not assigned, enquueue
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
            {   // only suport lwtt
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
            {   // only support ddjb
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
            {   // onlly support cfft
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
        { // no requirement, looking for gates with no special service
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
            //if still no gate found for flight with no requirements, accept any
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


    Console.WriteLine($"Total number of Flights and Boarding Gates processed and assigned: {flightsProcessed}");
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
    Console.WriteLine("B. Display the total fee per airline for the day (Advance)");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Please select your option:\n");

    string option = Console.ReadLine().ToUpper();
    if (option == "0")
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
        // Pierre
        // Feature 4 (Option 2)
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
        Console.WriteLine("");
        CreateFlight(terminal5);
    }
    else if (option == "5")
    {
        // Pierre
        // Feature 7 & 8 (Option 5/6)
        Console.WriteLine("");
        DisplayFullFlightOfAirline(terminal5);
    }
    else if (option == "6")
    {
        // Pierre
        // Feature 8 (Option 5/6)
        ModifyFlightDetails(terminal5);
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
    else if (option == "B" || option == "b")
    {
        // Pierre
        // Advanced Feature B
        Console.WriteLine("");
        DisplayTotalFeePerAirLine(terminal5);
    }
    else
    {
        Console.WriteLine("Invalid Choice!");
    }

}
