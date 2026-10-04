using Microsoft.EntityFrameworkCore;
using MovieWebAPI.DTOs;
using MovieWebAPI.Models;
using MovieWebAPI.Persistence;

namespace MovieWebAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext _dbContext;
        private readonly ILogger<MovieService> _logger;

        public MovieService(MovieDbContext dbContext, ILogger<MovieService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<MovieDto> CreateMovieAsync(CreateMovieDto movie)
        {
            var newMovie = Movie.Create(movie.Title, movie.Genre, movie.ReleaseDate, movie.Rating);

            await _dbContext.Movies.AddAsync(newMovie);
            await _dbContext.SaveChangesAsync();

            return new MovieDto
            (
                newMovie.Id,
                newMovie.Title,
                newMovie.Genre,
                newMovie.ReleaseDate,
                newMovie.Rating
            );
        }

        public async Task DeleteMovieAsync(Guid id)
        {
            var existingMovie = await _dbContext.Movies.FindAsync(id);

            if (existingMovie is null)
            {
                throw new ArgumentNullException($"Movie with ID {id} not found.");
            }

            _dbContext.Movies.Remove(existingMovie);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            return await _dbContext.Movies
                .AsNoTracking()
                .Select(m => new MovieDto
                (
                    m.Id,
                    m.Title,
                    m.Genre,
                    m.ReleaseDate,
                    m.Rating
                ))
                .ToListAsync();
        }

        public async Task<MovieDto?> GetMovieByIdAsync(Guid id)
        {
            return await _dbContext.Movies
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new MovieDto
                (
                    m.Id,
                    m.Title,
                    m.Genre,
                    m.ReleaseDate,
                    m.Rating
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesByGenreAsync(string genre)
        {
            return await _dbContext.Movies
                .AsNoTracking()
                .Where(m => m.Genre == genre)
                .Select(m => new MovieDto
                (
                    m.Id,
                    m.Title,
                    m.Genre,
                    m.ReleaseDate,
                    m.Rating
                ))
                .ToListAsync();
        }

        public async Task UpdateMovieAsync(Guid id, UpdateMovieDto movieUpdateDetails)
        {
            var existingMovie = await _dbContext.Movies.FindAsync(id);

            if (existingMovie is null)
            {
                throw new ArgumentNullException($"Movie with ID {id} not found.");
            }

            existingMovie.Update(movieUpdateDetails.Title, movieUpdateDetails.Genre, movieUpdateDetails.ReleaseDate, movieUpdateDetails.Rating);
            await _dbContext.SaveChangesAsync();
        }
    }
}
