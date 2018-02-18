namespace PhotoShare.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Services.Contracts;

    public class UserService : IUserService
    {
        private readonly PhotoShareContext context;

        public UserService(PhotoShareContext context)
        {
            this.context = context;
        }

        public User ByUsername(string username)
        {
            User user = this.context.Users
                .AsNoTracking()
                .Where(u => u.Username == username)
                .SingleOrDefault();

            return user;
        }

        public User ByUsernameWithFriends(string username)
        {
            User user = this.context.Users
                .Include(uf => uf.FriendsAdded)
                .AsNoTracking()
                .Where(u => u.Username == username)
                .SingleOrDefault();

            return user;
        }

        public User Create(string username, string password, string repeatPassword, string email)
        {
            User user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now,
                LastTimeLoggedIn = DateTime.Now
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();

            return user;
        }

        public void Modify(User user)
        {
            this.context.Update(user);
            this.context.SaveChanges();
        }

        public void DeleteByUsername(int userId)
        {
            User user = this.context.Users.Find(userId);

            user.IsDeleted = true;

            this.context.Users.Update(user);
            this.context.SaveChanges();
        }

        public Friendship CheckFriendship(int requestedUserId, int friendUserId)
        {
            Friendship friendship = this.context.Friendships
                .AsNoTracking()
                .Where(f => f.UserId == requestedUserId && f.FriendId == friendUserId)
                .SingleOrDefault();

            return friendship;
        }

        public void AcceptFriend(int requestedUserId, int friendUserId)
        {
            Friendship friendship = new Friendship
            {
                UserId = requestedUserId,
                FriendId = friendUserId
            };

            this.context.Friendships.Add(friendship);
            this.context.SaveChanges();
        }

        public void AddFriend(int requestedUserId, int friendUserId)
        {
            Friendship friendship = new Friendship
            {
                UserId = requestedUserId,
                FriendId = friendUserId
            };

            this.context.Friendships.Add(friendship);
            this.context.SaveChanges();
        }

        public ICollection<string> ListFriends(int userId)
        {
            ICollection<string> userFriends = this.context.Friendships
                .Include(u => u.Friend)
                .Where(u => u.UserId == userId)
                .Select(f => f.Friend.Username)
                .ToList();

            return userFriends;
        }
    }
}
