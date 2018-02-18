namespace BusTickets.Client
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class Application
    {
        public static void Main(string[] args)
        {
            using (BusTicketsContext context = new BusTicketsContext())
            {
                context.Database.Migrate();
                SeedData(context);
            }
        }

        private static void SeedData(BusTicketsContext context)
        {
            Town[] towns = new[]
            {
                new Town { Name = "Sofia", Country = "Bulgaria" },
                new Town { Name = "Veliko Tyrnovo", Country = "Bulgaria" },
                new Town { Name = "Varna", Country = "Bulgaria" },
                new Town { Name = "Burgas", Country = "Bulgaria" },
                new Town { Name = "Frankfurt", Country = "Germany" },
                new Town { Name = "Berlin", Country = "Germany" },
                new Town { Name = "Amsterdam", Country = "Netherland" }
            };
        }
    }
}