using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> Genres { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Movie>()
        .HasKey(m => m.MovieId);

    // Configure the relationship between Movie and MovieGenre
    modelBuilder.Entity<Movie>()
        .HasOne(m => m.Genre)          // Movie has one Genre
        .WithMany()                   // Genre can have many Movies (you can change this if needed)
        .HasForeignKey(m => m.GenreId) // FK is GenreId in Movie
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete if you want
}

    }
}

