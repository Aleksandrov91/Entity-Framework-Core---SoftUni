namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Client.Contracts;
    using Client.Sessions;
    using Client.Utilities;
    using Models;
    using Services.Contracts;

    public class AddTagToCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly ITagService tagService;

        public AddTagToCommand(IAlbumService albumService, ITagService tagService)
        {
            this.albumService = albumService;
            this.tagService = tagService;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(params string[] data)
        {
            string albumName = data[0];
            string tagName = TagUtilities.ValidateOrTransform(data[1]);

            Album album = this.albumService.ByTitle(albumName);
            Tag tag = this.tagService.ByName(tagName);
            bool isAlbumOwner = album.AlbumRoles
                .Any(ar => ar.Album == album && ar.User.Username == CurrentSession.LoggedUser.Username);

            if (CurrentSession.LoggedUser == null || !isAlbumOwner)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            if (album == null || tag == null)
            {
                throw new ArgumentException("Either tag or album do not exist!");
            }

            this.albumService.AddTag(album.Id, tag.Id);

            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
