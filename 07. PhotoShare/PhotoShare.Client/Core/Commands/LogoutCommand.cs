namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Sessions;

    public class LogoutCommand : ICommand
    {
        public string Execute(params string[] data)
        {
            if (CurrentSession.LoggedUser == null)
            {
                throw new ArgumentException("You should log in first in order to logout.");
            }

            string loggedUsername = CurrentSession.LoggedUser.Username;

            CurrentSession.LoggedUser = null;

            return $"User {loggedUsername} successfully logged out!";
        }
    }
}
