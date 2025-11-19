using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.IncrementBlogEntryViewCount
{
    public class IncrementBlogEntryViewCountCommand : BlogEntryDto, IRequest
    {
        public int BlogEntryId { get; set; }
    }
}
