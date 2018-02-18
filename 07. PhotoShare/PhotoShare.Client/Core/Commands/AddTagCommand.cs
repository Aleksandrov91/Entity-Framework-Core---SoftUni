namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Sessions;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class AddTagCommand : ICommand
    {
        private readonly ITagService tagService;

        public AddTagCommand(ITagService tagService)
        {
            this.tagService = tagService;
        }

        // AddTag <tag>
        public string Execute(string[] data)
        {
            string tagName = data[0].ValidateOrTransform();

            Tag tag = this.tagService.ByName(tagName);

            if (CurrentSession.LoggedUser == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            else if (tag != null)
            {
                throw new ArgumentException($"Tag {tagName} exists!");
            }

            this.tagService.Add(tagName);

            return $"Tag {tagName} was added successfully!";
        }
    }
}
