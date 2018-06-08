using System;
using Microsoft.Extensions.DependencyInjection;
using SearchFlights.Managers;

namespace SearchFlights
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var input = GetOriginDestination();

                var services = new ServiceCollection();
                services.AddTransient<IFlightManager, FlightManager>();
                var provider = services.BuildServiceProvider();
                var flightManager = provider.GetService<IFlightManager>();

                var result = flightManager.SearchFlights(origin: input.Item1, destination: input.Item2);

                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.ReadKey(true);
            }
        }

        static Tuple<string, string> GetOriginDestination()
        {
            Console.WriteLine("Search flights by origin and destination");
            var isValidInput = false;
            var origin = string.Empty;
            var destination = string.Empty;

            while (!isValidInput)
            {
                Console.WriteLine("Enter input in format: searchFlights -o {Origin} -d {Destination}");
                var line = Console.ReadLine();
                var input = line.Split(' ');
                if (input.Length == 5 && input[0].Equals("searchFlights") && input[1].Equals("-o") && input[3].Equals("-d"))
                {
                    isValidInput = true;
                    origin = input[2];
                    destination = input[4];
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }
            }
            return new Tuple<string, string>(origin, destination);
        }
    }
}