using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using SearchFlights.Common;
using SearchFlights.Models;

namespace SearchFlights.Managers
{
    public class FlightManager : IFlightManager
    {
        private static string rootPath = ConfigurationSettings.AppSettings["rootPath"];
        private string targetPath = rootPath + ConfigurationSettings.AppSettings["processedProviderFileName"];

        public FlightManager()
        {
            try
            {
                var sourcePath1 = rootPath + ConfigurationSettings.AppSettings["providerFileName1"];
                var sourcePath2 = rootPath + ConfigurationSettings.AppSettings["providerFileName2"];
                var sourcePath3 = rootPath + ConfigurationSettings.AppSettings["providerFileName3"];

                File.WriteAllLines(targetPath, File.ReadAllLines(sourcePath1).Skip(1));
                File.AppendAllLines(targetPath, File.ReadAllLines(sourcePath2).Skip(1));
                File.AppendAllLines(targetPath, File.ReadAllLines(sourcePath3).Skip(1));
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (IOException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string SearchFlights(string origin, string destination)
        {
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException();
            }

            var seperator = Constants.CommaSeperator;
            var departureDateFormat = Constants.DateFormat1;
            var destinationDateFormat = Constants.DateFormat1;
            var providerColumns = new string[5];
            var flights = new HashSet<Flight>();

            var providerOrigin = string.Empty;
            var providerDestination = string.Empty;
            var providerDepartureTime = string.Empty;
            var providerDestinationTime = string.Empty;
            var providerPrice = string.Empty;

            try
            {
                var streamReader = new StreamReader(targetPath);
                var line = streamReader.ReadLine();

                while (line != null)
                {
                    seperator = GetLineSeperator(line);
                    providerColumns = line.Split(seperator);

                    providerOrigin = providerColumns[0].Trim();
                    providerDepartureTime = providerColumns[1].Trim();
                    providerDestination = providerColumns[2].Trim();
                    providerDestinationTime = providerColumns[3].Trim();
                    providerPrice = providerColumns[4].Trim();

                    if (providerOrigin == origin && providerDestination == destination)
                    {
                        departureDateFormat = Constants.DateFormat1;
                        departureDateFormat = GetDateFormat(providerDepartureTime);

                        destinationDateFormat = Constants.DateFormat1;
                        destinationDateFormat = GetDateFormat(providerDestinationTime);

                        var flight = new Flight
                        {
                            Origin = providerOrigin,
                            DepartureTime = ConvertStringToDateTime(providerDepartureTime, departureDateFormat),
                            Destination = providerDestination,
                            DestinationTime = ConvertStringToDateTime(providerDestinationTime, destinationDateFormat),
                            Price = Convert.ToDouble(Convert.ToString(providerPrice).Replace("$", ""))
                        };
                        flights.Add(flight);
                    }
                    line = streamReader.ReadLine();
                }

                streamReader.Close();

                return ComputeResult(flights, origin, destination);
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (IOException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string ComputeResult(HashSet<Flight> flights, string origin, string destination)
        {
            try
            {
                if (flights.Count > 0)
                {
                    string result = string.Empty;

                    //to sort by price and then by departure time
                    flights = new HashSet<Flight>(flights.OrderBy(f => f.Price).ThenBy(f => f.DepartureTime));

                    foreach (var flight in flights)
                    {
                         result = result + $"{flight.Origin} --> {flight.Destination} ({flight.DepartureTime.ToString("M/dd/yyyy H:mm:ss")} --> {flight.DestinationTime.ToString("M/dd/yyyy H:mm:ss")}) - ${flight.Price} \n";
                    }
                    return result;
                }
                return $"No Flights Found for {origin} --> {destination}";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private DateTime ConvertStringToDateTime(string str, string dateFormat)
        {
            try
            {
                return DateTime.ParseExact(str, dateFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private char GetLineSeperator(string line)
        {
            try
            {
                if (line.Contains(Constants.CommaSeperator))
                {
                    return Constants.CommaSeperator;
                }
                else if (line.Contains(Constants.PipeSeperator))
                {
                    return Constants.PipeSeperator;
                }
                throw new System.FormatException("Invalid file format");
            }
            catch (FormatException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetDateFormat(string date)
        {
            try
            {
                if (date.Contains(Constants.DateFormatSeperator1))
                {
                    return Constants.DateFormat1;
                }
                else if (date.Contains(Constants.DateFormatSeperator2))
                {
                    return Constants.DateFormat2;
                }
                throw new System.FormatException("Invalid date format");
            }
            catch (FormatException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}