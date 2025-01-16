using PRG2_ASG;


string filePath = "flights.csv"; // Update with the correct file path
Dictionary<string, Flight> flightDictionary = new Dictionary<string, Flight>();

// Read the CSV file and create Flight objects
string[] lines = File.ReadAllLines(filePath);
for (int i = 1; i < lines.Length; i++) // Skip the header row
{
    string[] data = lines[i].Split(',');
    string FN = data[0];
    string Origin = data[1];
    string Destination = data[2];
    DateTime ExepectedTime = Convert.ToDateTime(data[3]);
    string Status = data[4];

}

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
}
