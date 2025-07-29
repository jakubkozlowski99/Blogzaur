using AutoMapper;
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
        public CommentMappingProfile()
        {
            CreateMap<CommentDto, Domain.Entities.Comment>()
                .ReverseMap();
        }
    }
}
