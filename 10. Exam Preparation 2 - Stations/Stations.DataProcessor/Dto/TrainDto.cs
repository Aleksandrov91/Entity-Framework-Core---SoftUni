namespace Stations.DataProcessor.Dto
{
    using System.Collections.Generic;

    using Models.Enums;
    using Newtonsoft.Json;

    public class TrainDto
    {
        public TrainDto()
        {
            this.TrainSeats = new List<TrainSeatDto>();
        }

        public string TrainNumber { get; set; }

        public TrainType? Type { get; set; }

        [JsonProperty("Seats")]
        public ICollection<TrainSeatDto> TrainSeats { get; set; }
    }
}
