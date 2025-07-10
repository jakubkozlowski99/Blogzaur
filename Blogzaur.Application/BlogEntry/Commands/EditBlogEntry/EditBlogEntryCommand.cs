using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.EditBlogEntry
{
    public class EditBlogEntryCommand : BlogEntryDto, IRequest
    {

    }
}
