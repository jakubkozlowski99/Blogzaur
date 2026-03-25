using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Category.Commands.RemoveCategory
{
    public class RemoveCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
