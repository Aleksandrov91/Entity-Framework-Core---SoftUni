namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class UploadPictureCommand : ICommand
    {
        private readonly IAlbumService albumService;

        public UploadPictureCommand(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public string Execute(string[] data)
        {
            string albumName = data[0];
            string pictureTitle = data[1];
            string picturePath = data[2];

            Album album = this.albumService.ByTitle(albumName);

            bool isOwner = album.AlbumRoles
                    .Any(ar => ar.Album == album && ar.User.Username == CurrentSession.LoggedUser.Username);

            if (!isOwner)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            else if (album == null)
            {
                throw new ArgumentException($"Album {albumName} not found!");
            }

            this.albumService.Upload(album.Id, pictureTitle, picturePath);

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}
