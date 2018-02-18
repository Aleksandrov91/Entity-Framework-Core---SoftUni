namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class AddTownCommand : ICommand
    {
        private readonly ITownService townService;

        public AddTownCommand(ITownService townService)
        {
            this.townService = townService;
        }

        // AddTown <townName> <countryName>
        public string Execute(string[] data)
        {
            string townName = data[0];
            string country = data[1];

            Town town = this.townService.ByName(townName);

            if (CurrentSession.LoggedUser == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            else if (town != null)
            {
                throw new ArgumentException($"Town {townName} was already added!");
            }

            this.townService.Add(townName, country);

            return $"Town {townName} was added successfully!";
            }
    }
}
