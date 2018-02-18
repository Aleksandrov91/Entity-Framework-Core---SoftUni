namespace PhotoShare.Services.Contracts
{
    using System.Collections.Generic;

    using Models;

    public interface IUserService
    {
        User ByUsername(string username);

        User Create(string username, string password, string repeatPassword, string email);

        User ByUsernameWithFriends(string username);

        void Modify(User user);

        void DeleteByUsername(int userId);

        Friendship CheckFriendship(int requestedUserId, int friendUserId);

        void AcceptFriend(int requestedUserId, int friendUserId);

        void AddFriend(int requestedUserId, int friendUserId);

        ICollection<string> ListFriends(int userId);
    }
}
