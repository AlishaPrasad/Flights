using System;

namespace SearchFlights.Models
{
    class Flight : IEquatable<Flight>
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime DestinationTime { get; set; }
        public double Price { get; set; }

        public bool Equals(Flight other)
        {
            if (other != null)
            {
                return Origin.Equals(other.Origin) &&
                    Destination.Equals(other.Destination) &&
                    DepartureTime.Equals(other.DepartureTime) &&
                    DestinationTime.Equals(other.DestinationTime) &&
                    Price.Equals(other.Price);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hOrigin = Origin != null ? Origin.GetHashCode() : 0;
            int hDestination = Destination != null ? Destination.GetHashCode() : 0;
            int hDepartureTime = DepartureTime != null ? DepartureTime.GetHashCode() : 0;
            int hDestinationTime = DestinationTime != null ? DestinationTime.GetHashCode() : 0;
            int hPrice = Price != 0 ? Price.GetHashCode() : 0;
            return hOrigin ^ hDestination ^ hDepartureTime ^ hDestinationTime ^ hPrice;
        }
    }
}