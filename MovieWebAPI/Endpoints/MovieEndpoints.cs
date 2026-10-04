using MovieWebAPI.DTOs;
using MovieWebAPI.Services;

namespace MovieWebAPI.Endpoints
{
    public static class MovieEndpoints
    {
        public static void MapMovieEndpoints(this IEndpointRouteBuilder routes)
        {
            var movieApi = routes.MapGroup("/api/movies").WithTags("Movie Endpoints");

            movieApi.MapPost("/", async (IMovieService movieService, CreateMovieDto newMovie) =>
            {
                var createdMovie = await movieService.CreateMovieAsync(newMovie);
                return TypedResults.Created($"/api/movies/{createdMovie.Id}", createdMovie);
            });

            movieApi.MapGet("/", async (IMovieService movieService) =>
            {
                var movies = await movieService.GetAllMoviesAsync();
                return TypedResults.Ok(movies);
            });

            //movieApi.MapGet("/{id}", GetMovieByIdAsync);

            //static async Task<IResult> GetMovieByIdAsync(IMovieService movieService, Guid id)
            //{
            //    var movie = await movieService.GetMovieByIdAsync(id);
            //    return movie is null
            //        ? TypedResults.NotFound(new { Message = $"Movie with ID {id} not found." })
            //        : TypedResults.Ok(movie);
            //}

            movieApi.MapGet("/{id}", async (IMovieService service, Guid id) =>
            {
                var movie = await service.GetMovieByIdAsync(id);

                return movie is null
                    ? (IResult)TypedResults.NotFound(new { Message = $"Movie with ID {id} not found." })
                    : TypedResults.Ok(movie);
            });

            movieApi.MapGet("/genre/{genre}", async (IMovieService service, string genre) =>
            {
                var movies = await service.GetMoviesByGenreAsync(genre);
                return TypedResults.Ok(movies);
            });

            movieApi.MapPut("/{id}", async (IMovieService service, Guid id, UpdateMovieDto command) =>
            {
                await service.UpdateMovieAsync(id, command);
                return TypedResults.NoContent();
            });

            movieApi.MapDelete("/{id}", async (IMovieService service, Guid id) =>
            {
                await service.DeleteMovieAsync(id);
                return TypedResults.NoContent();
            });
        }
    }
}
