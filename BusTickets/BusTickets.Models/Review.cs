namespace BusTickets.Models
{
    using System;

    public class Review
    {
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public string Content { get; set; }

        public double Grade { get; set; }

        public DateTime Published { get; set; }
    }
}
