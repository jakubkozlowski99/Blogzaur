using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries
{
    public class GetAllBlogEntriesQuery : IRequest<List<BlogEntryDto>>
    {

    }
}
