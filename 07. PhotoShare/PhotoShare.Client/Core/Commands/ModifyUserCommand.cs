namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class ModifyUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly ITownService townService;

        public ModifyUserCommand(IUserService userService, ITownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            string username = data[0];
            string property = data[1];
            string newValue = data[2];

            if (CurrentSession.LoggedUser.Username != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            User user = this.userService.ByUsername(username);

            if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            string methodName = $"Modify{property}".Trim();

            MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                throw new ArgumentException($"Property {property} not supported!");
            }

            method.Invoke(this, new object[] { user, newValue });

            return $"User {username} {property} is {newValue}.";
        }

        private void ModifyBornTown(User user, string newBornTown)
        {
            Town town = this.townService.ByName(newBornTown);

            if (town == null)
            {
                throw new ArgumentException($"Value {newBornTown} not valid.{Environment.NewLine}Town {newBornTown} not found!");
            }

            user.BornTown = town;

            this.userService.Modify(user);
        }

        private void ModifyCurrentTown(User user, string currentTown)
        {
            Town town = this.townService.ByName(currentTown);

            if (town == null)
            {
                throw new ArgumentException($"Value {currentTown} not valid.{Environment.NewLine}Town {currentTown} not found!");
            }

            user.CurrentTown = town;

            this.userService.Modify(user);
        }

        private void ModifyPassword(User user, string newPassword)
        {
            if (!newPassword.Any(c => char.IsDigit(c)) || !newPassword.Any(c => char.IsLower(c)))
            {
                throw new ArgumentException($"Value {newPassword} not valid.{Environment.NewLine}Invalid Password");
            }

            user.Password = newPassword;

            this.userService.Modify(user);
        }
    }
}
