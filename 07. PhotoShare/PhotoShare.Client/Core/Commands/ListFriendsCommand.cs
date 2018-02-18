namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class ListFriendsCommand : ICommand
    {
        private readonly IUserService userService;

        public ListFriendsCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // PrintFriendsList <username>
        public string Execute(string[] data)
        {
            string username = data[0];

            User user = this.userService.ByUsername(username);

            if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            ICollection<string> userFriends = this.userService.ListFriends(user.Id);

            if (userFriends.Count == 0)
            {
                return $"No friends for this user. :(";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Friends");

            foreach (var friendName in userFriends.OrderBy(f => f))
            {
                sb.AppendLine($"-{friendName}");
            }

            return sb.ToString().Trim();
        }
    }
}
