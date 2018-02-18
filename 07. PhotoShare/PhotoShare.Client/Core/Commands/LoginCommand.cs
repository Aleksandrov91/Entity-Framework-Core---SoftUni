namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class LoginCommand : ICommand
    {
        private readonly IUserService userService;

        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(params string[] data)
        {
            string username = data[0];
            string password = data[1];

            User user = this.userService.ByUsername(username);

            if (CurrentSession.LoggedUser != null)
            {
                throw new ArgumentException("You should logout first!");
            }
            else if (user == null || user.Password != password)
            {
                throw new ArgumentException("Invalid username or password!");
            }

            CurrentSession.LoggedUser = user;

            return $"User {username} successfully logged in!";
        }
    }
}
