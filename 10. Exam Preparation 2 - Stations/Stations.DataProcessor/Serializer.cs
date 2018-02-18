namespace Stations.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models.Enums;
    using Newtonsoft.Json;
    
    public class Serializer
    {
        public static string ExportDelayedTrains(StationsDbContext context, string dateAsString)
        {
            DateTime date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var delayedTrains = context.Trains
                .Where(t => t.Trips.Any(ts => ts.Status == Models.Enums.TripStatus.Delayed && ts.DepartureTime <= date))
                .Select(tr => new
                {
                    TrainNumber = tr.TrainNumber,
                    DelayedTimes = tr.Trips.Where(ts => ts.Status == Models.Enums.TripStatus.Delayed).Count(),
                    MaxDelayedTime = tr.Trips.Where(ts => ts.Status == Models.Enums.TripStatus.Delayed).Max(ts => ts.TimeDifference)
                })
                .ToArray();

            delayedTrains = delayedTrains.OrderByDescending(dt => dt.DelayedTimes)
                .ThenByDescending(dt => dt.MaxDelayedTime)
                .ThenBy(t => t.TrainNumber)
                .ToArray();

            var json = JsonConvert.SerializeObject(delayedTrains);

            return json.ToString();
        }

        public static string ExportCardsTicket(StationsDbContext context, string cardType)
        {
            CardType customerCard = Enum.Parse<CardType>(cardType);

            var ticketsFromCards = context.Tickets
                .Include(c => c.CustomerCard)
                .Include(tr => tr.Trip)
                .ThenInclude(s => s.OriginStation)
                .Include(tr => tr.Trip)
                .ThenInclude(s => s.DestinationStation)
                .Where(t => t.CustomerCard.Type == customerCard)
                .OrderBy(c => c.CustomerCard.Name)
                .GroupBy(cc => cc.CustomerCard)
                .ToArray();

            XDocument document = new XDocument(new XElement("Cards"));

            foreach (var customer in ticketsFromCards)
            {
                var cardNameAttribute = new XAttribute("name", customer.Key.Name);
                var cardTypeAttribute = new XAttribute("type", customer.Key.Type);
                var card = new XElement("Card", cardNameAttribute, cardTypeAttribute);
                document.Root.Add(card);
                var cardTickets = new XElement("Tickets");
                card.Add(cardTickets);

                foreach (var customerTickets in customer)
                {
                    var ticket = new XElement("Ticket",
                                 new XElement("OriginStation", customerTickets.Trip.OriginStation.Name),
                                 new XElement("DestinationStation", customerTickets.Trip.DestinationStation.Name),
                                 new XElement("DepartureTime", customerTickets.Trip.DepartureTime.ToString("dd/MM/yyyy HH:mm")));

                    cardTickets.Add(ticket);
                }
            }

            return document.ToString();
        }
    }
}