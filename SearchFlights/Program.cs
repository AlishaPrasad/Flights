using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SearchFlights.Managers;
using SearchFlights.Models;
using System.Configuration;

namespace SearchFlights
{ 
    class Program
    {
        static string origin = string.Empty;

        static string destination = string.Empty;

        static string rootPath = ConfigurationSettings.AppSettings["rootPath"];

        static void Main(string[] args)
        {
            try
            {
                string targetPath = rootPath + ConfigurationSettings.AppSettings["processedProviderFileName"];

                CombineProviderData(targetPath);

                AcceptInput();

                var services = new ServiceCollection();
                services.AddTransient<IFlightManager, FlightManager>();
                var provider = services.BuildServiceProvider();
                IFlightManager flightManager = provider.GetService<IFlightManager>();

                //hash set used to avoid duplicate results
                HashSet<Flight> flights = flightManager.SearchFlights(targetPath, origin, destination);

                DisplayResult(flights);

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        static void CombineProviderData(string targetPath)
        {
            string sourcePath1 = rootPath + ConfigurationSettings.AppSettings["processedProviderFileName"];
            string sourcePath2 = rootPath + ConfigurationSettings.AppSettings["processedProviderFileName"];
            string sourcePath3 = rootPath + ConfigurationSettings.AppSettings["processedProviderFileName"];

            File.WriteAllLines(targetPath, File.ReadAllLines(sourcePath1).Skip(1));
            File.AppendAllLines(targetPath, File.ReadAllLines(sourcePath2).Skip(1));
            File.AppendAllLines(targetPath, File.ReadAllLines(sourcePath3).Skip(1));
        }

        static void AcceptInput()
        {
            Console.WriteLine("Search flights by origin and destination");
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine("Enter input in format: searchFlights -o {Origin} -d {Destination}");
                string line = Console.ReadLine();
                string[] input = line.Split(' ');
                if (input.Length == 5 && input[0].Equals("searchFlights") && input[1].Equals("-o") && input[3].Equals("-d"))
                {
                    validInput = true;
                    origin = input[2];
                    destination = input[4];
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }
            }
        }

        static void DisplayResult(HashSet<Flight> flights)
        {
            if (flights.Count > 0)
            {
                //to sort by price and then by departure time
                flights = new HashSet<Flight>(flights.OrderBy(f => f.Price).ThenBy(f => f.DepartureTime));

                foreach (var flight in flights)
                {
                    Console.WriteLine(flight.Origin + 
                                        " --> " + 
                                        flight.Destination + 
                                        " (" + 
                                        flight.DepartureTime.ToString("M/dd/yyyy H:mm:ss") + 
                                        " --> " + 
                                        flight.DestinationTime.ToString("M/dd/yyyy H:mm:ss") + 
                                        ") - $" + 
                                        flight.Price);
                }
            }
            else
            {
                Console.WriteLine("No Flights Found for " + origin + " --> " + destination);
            }
        }
    }
}