using Microsoft.EntityFrameworkCore;
using MovieCatalogue_Alpha.Elements;

namespace MovieCatalogue_Alpha.Context
{
    namespace MovieCatalogue_Alpha.Context
    {
        public class CatalogueContext : DbContext
        {
            public DbSet<Actor> Actor { get; set; }
            public DbSet<Director> Director { get; set; }
            public DbSet<Movie> Movie { get; set; }
            public DbSet<MovieActor> MovieActor { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseMySQL("server=localhost;database=MovieCatalogue;user=root;password=;");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<MovieActor>()
                    .HasKey(ma => new { ma.MovieID, ma.ActorID });

                modelBuilder.Entity<MovieActor>()
                    .HasOne(ma => ma.Movie)
                    .WithMany(m => m.MovieActors)
                    .HasForeignKey(ma => ma.MovieID);

                modelBuilder.Entity<MovieActor>()
                    .HasOne(ma => ma.Actor)
                    .WithMany(a => a.MovieActors)
                    .HasForeignKey(ma => ma.ActorID);

                modelBuilder.Entity<Movie>()
                    .HasOne(m => m.MovieDirector)
                    .WithMany()
                    .HasForeignKey(m => m.DirectorID);
            }
        }
    }
}
