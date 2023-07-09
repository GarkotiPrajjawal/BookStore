using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Books> Books { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasNoKey();
            modelBuilder.Entity<Books>().HasNoKey();
            modelBuilder.Entity<Publisher>().HasNoKey();
            //modelBuilder.Entity<User>().HasNoKey();
            //base.OnModelCreating(modelBuilder);
        }*/

        }
}