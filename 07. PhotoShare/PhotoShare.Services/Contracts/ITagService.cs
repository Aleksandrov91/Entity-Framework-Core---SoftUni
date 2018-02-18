namespace PhotoShare.Services.Contracts
{
    using System.Collections.Generic;

    using Models;

    public interface ITagService
    {
        Tag ByName(string tagName);

        ICollection<Tag> ByName(string[] tagNames);

        Tag Add(string tagName);
    }
}
