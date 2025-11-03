using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry
{
    public class CreateBlogEntryCommand : BlogEntryDto, IRequest
    {
        public List<int> CategoryIds { get; set; } = new();
    }
}
