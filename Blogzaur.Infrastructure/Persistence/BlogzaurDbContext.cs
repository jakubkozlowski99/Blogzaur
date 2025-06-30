using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Infrastructure.Persistence
{
    public class BlogzaurDbContext : DbContext
    {
        public BlogzaurDbContext(DbContextOptions<BlogzaurDbContext> options)
            : base(options)
        {

        }

        public DbSet<Domain.Entities.User> Users { get; set; }
        public DbSet<Domain.Entities.BlogEntry> BlogEntries { get; set; }
    }
}
