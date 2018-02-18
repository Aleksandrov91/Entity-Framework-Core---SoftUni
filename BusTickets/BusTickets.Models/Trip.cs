namespace BusTickets.Models
{
    using System;

    public class Trip
    {
        public int TripId { get; set; }

        public DateTime DepartuteTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public string Status { get; set; }

        public int OriginStationId { get; set; }

        public BusStation OriginStation { get; set; }

        public int DestinationStationId { get; set; }

        public BusStation DestinationStation { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int TicketId { get; set; }

        public Ticket Ticket { get; set; }
    }
}