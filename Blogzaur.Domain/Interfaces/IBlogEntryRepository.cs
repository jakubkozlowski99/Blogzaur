using Blogzaur.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Interfaces
{
    public interface IBlogEntryRepository
    {
        Task Create(BlogEntry blogEntry);
    }
}
