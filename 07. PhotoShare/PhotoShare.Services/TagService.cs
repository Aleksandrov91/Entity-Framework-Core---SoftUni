namespace PhotoShare.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Services.Contracts;

    public class TagService : ITagService
    {
        private readonly PhotoShareContext context;

        public TagService(PhotoShareContext context)
        {
            this.context = context;
        }

        public Tag ByName(string tagName)
        {
            Tag tag = this.context.Tags
                .AsNoTracking()
                .Where(t => t.Name == tagName)
                .SingleOrDefault();

            return tag;
        }

        public ICollection<Tag> ByName(string[] tagNames)
        {
            ICollection<Tag> tags = this.context.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToArray();

            return tags;
        }

        public Tag Add(string tagName)
        {
            Tag tag = new Tag
            {
                Name = tagName
            };

            this.context.Tags.Add(tag);
            this.context.SaveChanges();

            return tag;
        }
    }
}
