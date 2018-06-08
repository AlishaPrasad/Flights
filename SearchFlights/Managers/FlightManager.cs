using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SearchFlights.Models;

namespace SearchFlights.Managers
{
    class FlightManager :IFlightManager
    {
        public HashSet<Flight> SearchFlights(string targetPath, string origin, string destination)
        {
            if (string.IsNullOrEmpty(targetPath) || string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination))
            {
                throw new ArgumentNullException();
            }

            char seperator = ',';
            string dateFormat = "M/dd/yyyy H:mm:ss";
            string[] providerColumns = new string[5];
            HashSet<Flight> flights = new HashSet<Flight>();

            StreamReader streamReader = new StreamReader(targetPath);
            String line = streamReader.ReadLine();

            while (line != null)
            {
                seperator = ',';
                seperator = line.Contains('|') ? '|' : seperator;
                providerColumns = line.Split(seperator);

                if (providerColumns[0] == origin && providerColumns[2] == destination)
                {
                    dateFormat = "M/dd/yyyy H:mm:ss";
                    dateFormat = providerColumns[1].Contains('-') ? "M-dd-yyyy H:mm:ss" : dateFormat;

                    Flight flight = new Flight();
                    flight.Origin = providerColumns[0];
                    flight.DepartureTime = ConvertStringToDateTime(providerColumns[1], dateFormat);
                    flight.Destination = providerColumns[2];
                    flight.DestinationTime = ConvertStringToDateTime(providerColumns[3], dateFormat);
                    flight.Price = Convert.ToDouble(Convert.ToString(providerColumns[4]).Replace("$", ""));
                    flights.Add(flight);
                }
                line = streamReader.ReadLine();
            }

            streamReader.Close();

            return flights;
        }

        private DateTime ConvertStringToDateTime(string str, string dateFormat)
        {
            return DateTime.ParseExact(str, dateFormat, CultureInfo.InvariantCulture);
        }
    }
}