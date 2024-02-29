using MovieSearchAPI.Models;
using System.Text.Json;

namespace MovieSearchAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "6be40308";

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string title)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?apikey=6be40308&s={title}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var searchResult = JsonSerializer.Deserialize<OmdbSearchResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return searchResult.Search ?? new List<Movie>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching movies: {ex.Message}");
                return new List<Movie>();
            }
        }

        public async Task<Movie> GetMovieDetailsAsync(string imdbId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?apikey=6be40308&i={imdbId}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var movie = JsonSerializer.Deserialize<Movie>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return movie;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting movie details: {ex.Message}");
                return null;
            }
        }
    }
}
