namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class AddFriendCommand : ICommand
    {
        private readonly IUserService userService;

        public AddFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AddFriend <username1> <username2>
        public string Execute(string[] data)
        {
            string username = data[0];
            string friendUsername = data[1];

            if (CurrentSession.LoggedUser.Username != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            User user = this.userService.ByUsernameWithFriends(username);
            User friend = this.userService.ByUsernameWithFriends(friendUsername);

            if (user == null || friend == null)
            {
                string missingUser = user == null ? username : friendUsername;
                throw new ArgumentException($"User {missingUser} not found!");
            }

            Friendship friendship = this.userService.CheckFriendship(user.Id, friend.Id);

            if (friendship != null)
            {
                throw new InvalidOperationException($"{friendUsername} is already a friend to {username}");
            }

            this.userService.AddFriend(user.Id, friend.Id);

            return $"Friend {friendUsername} added to {username}";
        }
    }
}
