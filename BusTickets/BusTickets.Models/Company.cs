namespace BusTickets.Models
{
    using System.Collections.Generic;

    public class Company
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Nationality { get; set; }

        public decimal Rating { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
