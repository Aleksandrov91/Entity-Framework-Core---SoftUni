namespace BusTickets.Models
{
    using System.Collections.Generic;

    public class Town
    {
        public int TownId { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public ICollection<BusStation> BusStations { get; set; } = new List<BusStation>();
    }
}