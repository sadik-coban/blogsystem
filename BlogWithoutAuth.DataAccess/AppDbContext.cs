using BlogWithoutAuth.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BlogWithoutAuth.DataAccess
{

    public class AppDbContext : DbContext
    {
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<PostTag> PostTag { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BlogWithoutAuthDBA;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Posts)
                .UsingEntity<PostTag>();
            modelBuilder.Entity<Post>()
                .Ignore(nameof(Post.TagsString));
        }

    }
}