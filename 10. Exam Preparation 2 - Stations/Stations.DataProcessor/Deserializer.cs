namespace Stations.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Data;
    using DataProcessor.Dto;
    using DataProcessor.Dto.Import;
    using Models;
    using Models.Enums;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            Station[] stations = JsonConvert.DeserializeObject<Station[]>(jsonString);
            List<Station> validStations = new List<Station>();
            StringBuilder sb = new StringBuilder();

            foreach (var station in stations)
            {
                if (station.Name == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                else if (station.Town == null)
                {
                    station.Town = station.Name;
                }

                if (station.Name.Length > 50 || station.Town.Length > 50)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                else if (validStations.Any(s => s.Name == station.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                validStations.Add(station);
                sb.AppendLine(string.Format(SuccessMessage, station.Name));
            }

            context.AddRange(validStations);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            SeatingClass[] seatingClasses = JsonConvert.DeserializeObject<SeatingClass[]>(jsonString);
            List<SeatingClass> validClasses = new List<SeatingClass>();

            StringBuilder sb = new StringBuilder();

            foreach (var seatingClass in seatingClasses)
            {
                if (seatingClass.Name == null || seatingClass.Abbreviation == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                else if (seatingClass.Name.Length > 30 || seatingClass.Abbreviation.Length != 2)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                else if (validClasses.Any(s => s.Name == seatingClass.Name || s.Abbreviation == seatingClass.Abbreviation))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                validClasses.Add(seatingClass);
                sb.AppendLine(string.Format(SuccessMessage, seatingClass.Name));
            }

            context.AddRange(validClasses);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            TrainDto[] trainsDto = JsonConvert.DeserializeObject<TrainDto[]>(jsonString, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            SeatingClass[] seatingClasses = context.SeatingClasses.ToArray();
            StringBuilder sb = new StringBuilder();
            List<Train> validTrains = new List<Train>();

            foreach (var trainModel in trainsDto)
            {
                bool isInvalid = false;
                if (trainModel.TrainNumber == null || trainModel.TrainNumber.Length > 10 || validTrains.Any(t => t.TrainNumber == trainModel.TrainNumber))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                List<TrainSeat> trainSeats = new List<TrainSeat>();

                foreach (var seat in trainModel.TrainSeats)
                {
                    if (seat.Quantity <= 0)
                    {
                        sb.AppendLine(FailureMessage);
                        isInvalid = true;
                        break;
                    }

                    int seatingClassId = seatingClasses
                        .Where(sc => sc.Name == seat.Name && sc.Abbreviation == seat.Abbreviation)
                        .Select(sc => sc.Id)
                        .SingleOrDefault();

                    if (seatingClassId == 0 || trainSeats.Any(sc => sc.SeatingClassId == seatingClassId))
                    {
                        sb.AppendLine(FailureMessage);
                        isInvalid = true;
                        break;
                    }

                    TrainSeat trainSeat = new TrainSeat
                    {
                        SeatingClassId = seatingClassId,
                        Quantity = seat.Quantity
                    };

                    trainSeats.Add(trainSeat);
                }

                if (isInvalid)
                {
                    continue;
                }

                var train = new Train
                {
                    TrainNumber = trainModel.TrainNumber,
                    Type = trainModel.Type,
                    TrainSeats = trainSeats
                };

                validTrains.Add(train);
                sb.AppendLine(string.Format(SuccessMessage, train.TrainNumber));
            }

            context.AddRange(validTrains);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            TripDto[] deserialisedTrips = JsonConvert.DeserializeObject<TripDto[]>(jsonString);

            List<Trip> trips = new List<Trip>();

            foreach (var tripDto in deserialisedTrips)
            {
                if (!IsValid(tripDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Train train = context.Trains.SingleOrDefault(t => t.TrainNumber == tripDto.Train);
                Station originStation = context.Stations.SingleOrDefault(st => st.Name == tripDto.OriginStation);
                Station destinationStation = context.Stations.SingleOrDefault(st => st.Name == tripDto.DestinationStation);

                if (train == null || originStation == null || destinationStation == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime departuteTime = DateTime.ParseExact(tripDto.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime arrivalTime = DateTime.ParseExact(tripDto.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                if (arrivalTime < departuteTime)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                TimeSpan timeDifference;

                if (tripDto.TimeDifference != null)
                {
                    timeDifference = TimeSpan.ParseExact(tripDto.TimeDifference, @"hh\:mm", CultureInfo.InvariantCulture);
                }

                Trip trip = new Trip
                {
                    Train = train,
                    OriginStation = originStation,
                    DestinationStation = destinationStation,
                    DepartureTime = departuteTime,
                    ArrivalTime = arrivalTime,
                    Status = tripDto.Status,
                    TimeDifference = timeDifference
                };

                trips.Add(trip);
                sb.AppendLine($"Trip from {tripDto.OriginStation} to {tripDto.DestinationStation} imported.");
            }

            context.Trips.AddRange(trips);
            context.SaveChanges();

            string result = sb.ToString();
            return result;
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XDocument doc = XDocument.Parse(xmlString);
            List<CustomerCard> customerCards = new List<CustomerCard>();

            var elements = doc.Root.Elements();

            foreach (var element in elements)
            {
                string name = element.Element("Name").Value;
                int age = int.Parse(element.Element("Age").Value);
                string cardTypeString = element.Element("CardType")?.Value;
                CardType cardType = CardType.Normal;

                if (name.Length > 128)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (age < 0 || age > 120)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (cardTypeString != null)
                {
                    Enum.TryParse(cardTypeString, out cardType);
                }

                CustomerCard card = new CustomerCard
                {
                    Name = name,
                    Age = age,
                    Type = cardType
                };

                customerCards.Add(card);
                sb.AppendLine(string.Format(SuccessMessage, name));
            }

            context.AddRange(customerCards);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
            TicketDto[] deserializedTickets = (TicketDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            StringBuilder sb = new StringBuilder();

            List<Ticket> tickets = new List<Ticket>();

            foreach (var ticketDto in deserializedTickets)
            {
                if (!IsValid(ticketDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime departureTime = DateTime.ParseExact(ticketDto.Trip.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                Trip trip = context.Trips
                    .Where(t => t.OriginStation.Name == ticketDto.Trip.OriginStation &&
                           t.DestinationStation.Name == ticketDto.Trip.DestinationStation &&
                           t.DepartureTime == departureTime)
                    .SingleOrDefault();

                if (trip == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                CustomerCard card = null;

                if (ticketDto.Card != null)
                {
                    card = context.Cards
                    .Where(c => c.Name == ticketDto.Card.Name)
                    .SingleOrDefault();

                    if (card == null)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }

                string seatingAbbreviation = ticketDto.Seat.Substring(0, 2);
                int quantity = int.Parse(ticketDto.Seat.Substring(2));

                TrainSeat seatExist = trip.Train.TrainSeats
                    .SingleOrDefault(s => s.SeatingClass.Abbreviation == seatingAbbreviation && quantity <= s.Quantity);

                if (seatExist == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Ticket ticket = new Ticket
                {
                    Price = ticketDto.Price,
                    SeatingPlace = ticketDto.Seat,
                    Trip = trip,
                    CustomerCard = card
                };

                tickets.Add(ticket);
                sb.AppendLine($"Ticket from {ticketDto.Trip.OriginStation} to {ticketDto.Trip.DestinationStation} departing at {ticketDto.Trip.DepartureTime} imported.");
            }

            context.Tickets.AddRange(tickets);
            context.SaveChanges();

            string result = sb.ToString();
            return result;
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);

            List<ValidationResult> validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);

            return isValid;
        }
    }
}