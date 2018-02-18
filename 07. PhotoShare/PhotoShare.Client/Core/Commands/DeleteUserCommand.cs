namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class DeleteUserCommand : ICommand
    {
        private readonly IUserService serviceProvider;

        public DeleteUserCommand(IUserService serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        // DeleteUser <username>
        public string Execute(string[] data)
        {
            string username = data[0];
            User user = this.serviceProvider.ByUsername(username);

            if (CurrentSession.LoggedUser.Username != user.Username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            else if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }
            else if (user.IsDeleted == true)
            {
                throw new InvalidOperationException($"User {username} is already deleted!");
            }

            this.serviceProvider.DeleteByUsername(user.Id);

            return $"User {username} was deleted from the database!";
        }
    }
}
