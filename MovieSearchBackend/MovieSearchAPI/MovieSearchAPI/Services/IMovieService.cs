using MovieSearchAPI.Models;

namespace MovieSearchAPI.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> SearchMoviesAsync(string title);
        Task<Movie> GetMovieDetailsAsync(string imdbId);
    }
}
