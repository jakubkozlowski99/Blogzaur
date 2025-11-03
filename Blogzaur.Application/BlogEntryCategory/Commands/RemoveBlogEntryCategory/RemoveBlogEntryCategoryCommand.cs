using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntryCategory.Commands.RemoveBlogEntryCategory
{
    public class RemoveBlogEntryCategoryCommand : IRequest
    {
        public int BlogEntryId { get; set; }
        public int CategoryId { get; set; }
    }
}
