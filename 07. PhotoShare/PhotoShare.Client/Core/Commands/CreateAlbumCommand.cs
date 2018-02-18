namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;

    public class CreateAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ITagService tagService;

        public CreateAlbumCommand(IAlbumService albumService, IUserService userService, ITagService tagService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
        }

        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(params string[] data)
        {
            string username = data[0];
            string albumTitle = data[1];
            string bgColor = data[2];
            string[] tags = data.Skip(3).ToArray();

            User user = this.userService.ByUsername(username);
            Album album = this.albumService.ByTitle(albumTitle);
            ICollection<Tag> storedTags = this.tagService.ByName(tags);
            ICollection<int> tagIds = storedTags
                .Select(t => t.Id)
                .ToArray();
            bool isValidColor = Enum.TryParse(bgColor, true, out Color backgroundColor);

            if (CurrentSession.LoggedUser.Username != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            else if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }
            else if (album != null)
            {
                throw new ArgumentException($"Album {album} exists!");
            }
            else if (!isValidColor)
            {
                throw new ArgumentException($"Color {bgColor} not found!");
            }
            else if (storedTags.Count != tags.Length)
            {
                throw new ArgumentException("Invalid tags!");
            }

            this.albumService.Create(user.Id, albumTitle, backgroundColor, tagIds);

            return $"Album {albumTitle} successfully created!";
        }
    }
}
