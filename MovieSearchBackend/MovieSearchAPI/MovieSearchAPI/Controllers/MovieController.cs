using Microsoft.AspNetCore.Mvc;
using MovieSearchAPI.Services;
using Newtonsoft.Json;

namespace MovieSearchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ISearchHistoryService _searchHistoryService;

        public MovieController(IMovieService movieService, ISearchHistoryService searchHistoryService)
        {
            _movieService = movieService;
            _searchHistoryService = searchHistoryService;
        }



        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                    return BadRequest("Title cannot be empty");

                var movies = await _movieService.SearchMoviesAsync(title);

                _searchHistoryService.AddToSearchHistory(title);

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("history")]
        public IActionResult GetSearchHistory()
        {
            try
            {
                var searchHistory = _searchHistoryService.GetSearchHistory();
                Console.WriteLine("Search History: " + JsonConvert.SerializeObject(searchHistory));
                return Ok(searchHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching search history: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("results")]
        public async Task<IActionResult> ShowSearchResults(string title)
        {
            try
            {
                var movies = await _movieService.SearchMoviesAsync(title);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("details/{imdbID}")]
        public async Task<IActionResult> ShowMovieDetails(string imdbID)
        {
            try
            {
                var movie = await _movieService.GetMovieDetailsAsync(imdbID);
                if (movie == null)
                    return NotFound();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
