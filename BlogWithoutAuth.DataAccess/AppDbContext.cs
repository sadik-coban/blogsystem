using BlogWithoutAuth.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BlogWithoutAuth.DataAccess
{

    public class AppDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BlogWithoutAuthDBA;Trusted_Connection=True;");
            //.UseLazyLoadingProxies()
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Posts)
                .UsingEntity<PostTag>();
            modelBuilder.Entity<Post>()
                .Ignore(nameof(Post.TagIds));
        }

    }
}


//public virtual DbSet<PostTag> PostTag { get; set; }