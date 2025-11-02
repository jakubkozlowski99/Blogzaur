using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Mappings
{
    public class BlogEntryCategoryMappingProfile : Profile
    {
        public BlogEntryCategoryMappingProfile()
        {
            CreateMap<Domain.Entities.BlogEntryCategory, BlogEntryCategory.BlogEntryCategoryDto>().ReverseMap();
        }
    }
}
