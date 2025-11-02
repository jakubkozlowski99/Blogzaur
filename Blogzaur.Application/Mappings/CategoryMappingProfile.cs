using AutoMapper;
using Blogzaur.Application.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile() 
        { 
            CreateMap<Domain.Entities.Category, CategoryDto>();
        }
    }
}
