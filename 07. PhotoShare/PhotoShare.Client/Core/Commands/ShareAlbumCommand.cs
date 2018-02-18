namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class ShareAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;

        public ShareAlbumCommand(IAlbumService albumService, IUserService userService)
        {
            this.albumService = albumService;
            this.userService = userService;
        }

        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public string Execute(string[] data)
        {
            int albumId = int.Parse(data[0]);
            string username = data[1];
            string permission = data[2];

            Album album = this.albumService.ById(albumId);
            User user = this.userService.ByUsername(username);
            bool isHavePermission = Enum.TryParse(permission, true, out Role role);
            bool isOwner = album.AlbumRoles
                    .Any(ar => ar.Album == album && ar.User.Username == CurrentSession.LoggedUser.Username);

            if (!isOwner)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            else if (album == null)
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }
            else if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }
            else if (!isHavePermission)
            {
                throw new ArgumentException("Permission must be either \"Owner\" or \"Viewer\"!");
            }

            this.albumService.Share(album.Id, user.Id, role);

            return $"Username {username} added to album {album.Name} ({permission})";
        }
    }
}
