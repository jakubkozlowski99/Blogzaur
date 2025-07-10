using AutoMapper;
using Blogzaur.Application.BlogEntry;
using Blogzaur.Application.BlogEntry.Commands.EditBlogEntry;
using Blogzaur.Domain.Entities;

namespace Blogzaur.Application.Mappings
{
    public class BlogEntryMappingProfile : Profile
    {
        public BlogEntryMappingProfile()
        {
            CreateMap<BlogEntryDto, Domain.Entities.BlogEntry>();

            CreateMap<Domain.Entities.BlogEntry, BlogEntryDto>();

            CreateMap<BlogEntryDto, EditBlogEntryCommand>();
        }
    }
}
