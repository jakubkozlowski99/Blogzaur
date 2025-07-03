using Blogzaur.Application.BlogEntry;
using Blogzaur.Domain.Entities;

namespace Blogzaur.Application.Services
{
    public interface IBlogEntryService
    {
        Task Create(BlogEntryDto blogEntryDto);
    }
}