namespace Stations.Models
{
    using System.Collections.Generic;

    using Models.Enums;

    public class Train
    {
        public int Id { get; set; }

        public string TrainNumber { get; set; }

        public TrainType? Type { get; set; }

        public ICollection<TrainSeat> TrainSeats { get; set; }

        public ICollection<Trip> Trips { get; set; }
    }
}
