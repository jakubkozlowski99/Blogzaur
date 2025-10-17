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
        public DbSet<Domain.Entities.UserCommentLike> UserCommentLikes { get; set; }
        public DbSet<Domain.Entities.UserBlogEntryLike> UserBlogEntryLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Domain.Entities.BlogEntry>()
                .HasMany(e => e.Comments)
                .WithOne(c => c.BlogEntry)
                .HasForeignKey(c => c.BlogEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Domain.Entities.UserCommentLike>()
                .HasKey(ul => new { ul.UserId, ul.CommentId });

            builder.Entity<Domain.Entities.UserBlogEntryLike>()
                .HasKey(ul => new { ul.UserId, ul.BlogEntryId });

        }
    }
}
