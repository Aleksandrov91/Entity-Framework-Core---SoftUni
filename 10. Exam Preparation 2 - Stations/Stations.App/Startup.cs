namespace Stations.App
{
    using System;
    using System.IO;

    using AutoMapper;
    using Data;

    public class Startup
    {
        public static void Main(string[] args)
        {
            var context = new StationsDbContext();
            ResetDatabase(context);

            Console.WriteLine("Database Reset.");

            Mapper.Initialize(cfg => cfg.AddProfile<StationsProfile>());

            ImportEntities(context);
            ExportEntities(context);
        }

        private static void ImportEntities(StationsDbContext context, string baseDir = @"..\Datasets\")
        {
            const string ExportDir = "./ImportResults/";

            var stations = DataProcessor.Deserializer.ImportStations(context, File.ReadAllText(baseDir + "stations.json"));
            PrintAndExportEntityToFile(stations, ExportDir + "Stations.txt");

            var classes = DataProcessor.Deserializer.ImportClasses(context, File.ReadAllText(baseDir + "classes.json"));
            PrintAndExportEntityToFile(classes, ExportDir + "Classes.txt");

            var trains = DataProcessor.Deserializer.ImportTrains(context, File.ReadAllText(baseDir + "trains.json"));
            PrintAndExportEntityToFile(trains, ExportDir + "Trains.txt");

            var trips = DataProcessor.Deserializer.ImportTrips(context, File.ReadAllText(baseDir + "trips.json"));
            PrintAndExportEntityToFile(trips, ExportDir + "Trips.txt");

            var cards = DataProcessor.Deserializer.ImportCards(context, File.ReadAllText(baseDir + "cards.xml"));
            PrintAndExportEntityToFile(cards, ExportDir + "Cards.txt");

            var tickets = DataProcessor.Deserializer.ImportTickets(context, File.ReadAllText(baseDir + "tickets.xml"));
            PrintAndExportEntityToFile(tickets, ExportDir + "Tickets.txt");
        }

        private static void ExportEntities(StationsDbContext context)
        {
            const string ExportDir = "./ImportResults/";

            var jsonOutput = DataProcessor.Serializer.ExportDelayedTrains(context, "01/01/2017");
            Console.WriteLine(jsonOutput);
            File.WriteAllText(ExportDir + "DelayedTrains.json", jsonOutput);

            var xmlOutput = DataProcessor.Serializer.ExportCardsTicket(context, "Elder");
            Console.WriteLine(xmlOutput);
            File.WriteAllText(ExportDir + "CardsTicket.xml", xmlOutput);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static void ResetDatabase(StationsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}