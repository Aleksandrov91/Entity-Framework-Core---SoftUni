namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class RegisterUserCommand : ICommand
    {
        private readonly IUserService userService;

        public RegisterUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(params string[] data)
        {
            if (data.Length < 4)
            {
                throw new InvalidOperationException($"Command {GetType().Name} not valid!");
            }

            string username = data[0];
            string password = data[1];
            string repeatPassword = data[2];
            string email = data[3];

            User user = this.userService.ByUsername(username);

            if (CurrentSession.LoggedUser != null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            else if (user != null)
            {
                throw new ArgumentException($"Username {username} is already taken!");
            }
            else if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            this.userService.Create(username, password, repeatPassword, email);

            return "User " + username + " was registered successfully!";
        }
    }
}
