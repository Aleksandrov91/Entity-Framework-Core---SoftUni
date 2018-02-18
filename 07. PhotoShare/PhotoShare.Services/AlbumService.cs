namespace PhotoShare.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Services.Contracts;

    public class AlbumService : IAlbumService
    {
        private readonly PhotoShareContext context;

        public AlbumService(PhotoShareContext context)
        {
            this.context = context;
        }

        public Album ById(int id)
        {
            Album album = this.context.Albums
                .Include(ar => ar.AlbumRoles)
                .Where(a => a.Id == id)
                .SingleOrDefault();

            return album;
        }

        public Album ByTitle(string albumName)
        {
            Album album = this.context.Albums
                .Include(ar => ar.AlbumRoles)
                .Where(a => a.Name == albumName)
                .SingleOrDefault();

            return album;
        }

        public AlbumTag AddTag(int albumId, int tagId)
        {
            AlbumTag albumTag = new AlbumTag
            {
                TagId = tagId
            };

            this.context.AlbumTags.Add(albumTag);
            this.context.SaveChanges();

            return albumTag;
        }

        public void Create(int userId, string albumTitle, Color backgroundColor, ICollection<int> tags)
        {
            ICollection<AlbumTag> albumTags = tags
                .Select(t => new AlbumTag
                {
                    TagId = t
                })
                .ToList();

            Album album = new Album
            {
                Name = albumTitle,
                BackgroundColor = backgroundColor,
                AlbumTags = albumTags
            };

            AlbumRole albumRole = new AlbumRole
            {
                UserId = userId,
                Album = album,
                Role = Role.Owner
            };

            this.context.Albums.Add(album);
            this.context.AlbumRoles.Add(albumRole);
            this.context.SaveChanges();
        }

        public void Share(int albumId, int userId, Role role)
        {
            AlbumRole albumRole = new AlbumRole
            {
                UserId = userId,
                AlbumId = albumId,
                Role = role
            };

            this.context.AlbumRoles.Add(albumRole);
            this.context.SaveChanges();
        }

        public Picture Upload(int albumId, string pictureTitle, string picturePath)
        {
            Picture picture = new Picture
            {
                Title = pictureTitle,
                Path = picturePath,
                AlbumId = albumId
            };

            this.context.Pictures.Add(picture);
            this.context.SaveChanges();

            return picture;
        }
    }
}
