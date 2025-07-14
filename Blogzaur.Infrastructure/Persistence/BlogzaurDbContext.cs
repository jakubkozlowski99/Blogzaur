using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Infrastructure.Persistence
{
    public class BlogzaurDbContext : IdentityDbContext
    {
        public BlogzaurDbContext(DbContextOptions<BlogzaurDbContext> options)
            : base(options)
        {

        }
        public DbSet<Domain.Entities.BlogEntry> BlogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
