using System.Collections.Generic;
using SearchFlights.Models;

namespace SearchFlights.Managers
{
    interface IFlightManager
    {
        string SearchFlights(string origin, string destination);
    }
}