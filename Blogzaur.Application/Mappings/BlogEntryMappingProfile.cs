using AutoMapper;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Application.BlogEntry;
using Blogzaur.Application.BlogEntry.Commands.EditBlogEntry;
using Blogzaur.Application.Comment;
using Blogzaur.Domain.Entities;

namespace Blogzaur.Application.Mappings
{
    public class BlogEntryMappingProfile : Profile
    {
        public BlogEntryMappingProfile(IUserContext userContext)
        {
            var user = userContext.GetCurrentUser();

            CreateMap<BlogEntryDto, Domain.Entities.BlogEntry>();

            CreateMap<Domain.Entities.BlogEntry, BlogEntryDto>()
                .ForMember(dto => dto.isEditable, opt => opt.MapFrom(src => user != null && (src.AuthorId == user.Id || user.IsInRole("Moderator"))));

            CreateMap<BlogEntryDto, EditBlogEntryCommand>();
        }
    }
}
