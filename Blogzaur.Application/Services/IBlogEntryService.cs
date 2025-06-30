using Blogzaur.Domain.Entities;

namespace Blogzaur.Application.Services
{
    public interface IBlogEntryService
    {
        Task Create(BlogEntry blogEntry);
    }
}