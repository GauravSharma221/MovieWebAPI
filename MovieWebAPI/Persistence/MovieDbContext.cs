using Microsoft.EntityFrameworkCore;
using MovieWebAPI.Models;

namespace MovieWebAPI.Persistence
{
    public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies => Set<Movie>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    //var sampleMovies = new List<Movie>
                    //{
                    //    new Movie("Sonic The Hedgehog 3", "Fantasy", new DateTimeOffset(new DateTime(2025, 1, 3), TimeSpan.Zero), 7.5),
                    //    new Movie("The Shawshank Redemption", "Drama", new DateTimeOffset(1994, 9, 22, 0, 0, 0, TimeSpan.Zero), 9.3),
                    //    new Movie("The Godfather", "Crime", new DateTimeOffset(1972, 3, 24, 0, 0, 0, TimeSpan.Zero), 9.2),
                    //    new Movie("The Dark Knight", "Action", new DateTimeOffset(2008, 7, 18, 0, 0, 0, TimeSpan.Zero), 9.0),
                    //    new Movie("Pulp Fiction", "Crime", new DateTimeOffset(1994, 10, 14, 0, 0, 0, TimeSpan.Zero), 8.9),
                    //    new Movie("Forrest Gump", "Drama", new DateTimeOffset(1994, 7, 6, 0, 0, 0, TimeSpan.Zero), 8.8)
                    //};

                    var sampleMovie = await context.Set<Movie>().FirstOrDefaultAsync(b => b.Title == "The Shawshank Redemption");
                    if (sampleMovie is null)
                    {
                        sampleMovie = Movie.Create("The Shawshank Redemption", "Drama", new DateTimeOffset(new DateTime(1994, 9, 22), TimeSpan.Zero), 9.3);
                        await context.Set<Movie>().AddAsync(sampleMovie);
                        await context.SaveChangesAsync();
                    }
                })
                .UseSeeding((context, _) =>
                {
                    var sampleMovie = context.Set<Movie>().FirstOrDefault(b => b.Title == "The Shawshank Redemption");
                    if (sampleMovie == null)
                    {
                        sampleMovie = Movie.Create("The Shawshank Redemption", "Drama", new DateTimeOffset(new DateTime(1994, 9, 22), TimeSpan.Zero), 9.3);
                        context.Set<Movie>().Add(sampleMovie);
                        context.SaveChanges();
                    }
                });

        }
    }
   
}
