using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogzaur.Infrastructure.Persistence
{
    public class BlogzaurDbContext : IdentityDbContext
    {
        public BlogzaurDbContext(DbContextOptions<BlogzaurDbContext> options)
            : base(options)
        {

        }
        public DbSet<Domain.Entities.BlogEntry> BlogEntries { get; set; }
        public DbSet<Domain.Entities.Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Domain.Entities.BlogEntry>()
                .HasMany(e => e.Comments)
                .WithOne(c => c.BlogEntry)
                .HasForeignKey(c => c.BlogEntryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
