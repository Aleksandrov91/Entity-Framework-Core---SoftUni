namespace PhotoShare.Services.Contracts
{
    using System.Collections.Generic;

    using Models;

    public interface IAlbumService
    {
        Album ById(int id);

        Album ByTitle(string albumName);

        void Create(int userId, string albumTitle, Color backgroundColor, ICollection<int> tagIds);

        AlbumTag AddTag(int albumId, int tagId);

        void Share(int albumId, int userId, Role role);

        Picture Upload(int albumId, string pictureTitle, string picturePath);
    }
}
