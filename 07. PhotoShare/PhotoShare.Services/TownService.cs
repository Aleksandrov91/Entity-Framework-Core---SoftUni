namespace PhotoShare.Services
{
    using System.Linq;

    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Services.Contracts;

    public class TownService : ITownService
    {
        private readonly PhotoShareContext context;

        public TownService(PhotoShareContext context)
        {
            this.context = context;
        }

        public Town ByName(string townName)
        {
            Town town = this.context.Towns
                    .AsNoTracking()
                    .Where(t => t.Name == townName)
                    .SingleOrDefault();

            return town;
        }

        public Town Add(string townName, string countryName)
        {
            Town town = new Town
            {
                Name = townName,
                Country = countryName
            };

            this.context.Towns.Add(town);
            this.context.SaveChanges();

            return town;
        }
    }
}
