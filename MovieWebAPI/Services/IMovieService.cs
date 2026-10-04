using MovieWebAPI.DTOs;

namespace MovieWebAPI.Services
{
    public interface IMovieService
    {
        Task<MovieDto> CreateMovieAsync(CreateMovieDto movie);
        Task<MovieDto?> GetMovieByIdAsync(Guid id);
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
        Task<IEnumerable<MovieDto>> GetMoviesByGenreAsync(string genre);
        Task UpdateMovieAsync(Guid id, UpdateMovieDto movie);
        Task DeleteMovieAsync(Guid id);
    }
}
