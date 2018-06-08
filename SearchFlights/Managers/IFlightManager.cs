using System.Collections.Generic;
using SearchFlights.Models;

namespace SearchFlights.Managers
{
    interface IFlightManager
    {
        HashSet<Flight> SearchFlights(string targetPath, string origin, string destination);
    }
}