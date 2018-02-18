namespace BusTickets.Models
{
    using System.Collections.Generic;

    public class BusStation
    {
        public int StationId { get; set; }

        public string Name { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public ICollection<Trip> OriginTrips { get; set; } = new List<Trip>();

        public ICollection<Trip> DestinationTrips { get; set; } = new List<Trip>();
    }
}