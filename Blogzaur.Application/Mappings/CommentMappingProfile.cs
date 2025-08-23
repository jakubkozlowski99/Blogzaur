using AutoMapper;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Application.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Mappings
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile(IUserContext userContext)
        {
            CreateMap<CommentDto, Domain.Entities.Comment>()
                .ReverseMap()
                .ForMember(dto => dto.AuthorName, opt => opt.MapFrom(src => userContext.GetUserById(src.AuthorId).Username));

            //TODO: reformat created at date to more friendly format
        }
    }
}
