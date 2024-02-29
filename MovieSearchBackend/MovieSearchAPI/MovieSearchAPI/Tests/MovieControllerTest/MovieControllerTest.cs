using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieSearchAPI.Controllers;
using MovieSearchAPI.Models;
using MovieSearchAPI.Services;
using Xunit;

namespace MovieSearchAPI.Tests.MovieControllerTest
{
    public class MovieControllerTests
    {
        [Fact]
        public async Task Search_Returns_OK_With_Movies()
        {
            // Arrange
            var mockMovieService = new Mock<IMovieService>();
            var mockSearchHistoryService = new Mock<ISearchHistoryService>();
            var controller = new MovieController(mockMovieService.Object, mockSearchHistoryService.Object);
            var title = "Avatar";
            var movies = new List<Movie> { new Movie { Title = "Avatar", Year = "2009", Type = "Movie", Poster = "https://avatar.com/poster" } };
            mockMovieService.Setup(service => service.SearchMoviesAsync(title))
                            .ReturnsAsync(movies);

            // Act
            var result = await controller.Search(title);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMovies = Assert.IsAssignableFrom<IEnumerable<Movie>>(okResult.Value);
            Assert.Equal(movies, returnedMovies);
        }

        [Fact]
        public async Task Search_Returns_BadRequest_When_Title_Is_Null()
        {
            // Arrange
            var mockMovieService = new Mock<IMovieService>();
            var mockSearchHistoryService = new Mock<ISearchHistoryService>();
            var controller = new MovieController(mockMovieService.Object, mockSearchHistoryService.Object);
            string title = null;

            // Act
            var result = await controller.Search(title);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Title cannot be empty", badRequestResult.Value);
        }

        [Fact]
        public async Task Search_Returns_InternalServerError_On_Exception()
        {
            // Arrange
            var mockMovieService = new Mock<IMovieService>();
            var mockSearchHistoryService = new Mock<ISearchHistoryService>();
            var controller = new MovieController(mockMovieService.Object, mockSearchHistoryService.Object);
            var title = "Avatar";
            mockMovieService.Setup(service => service.SearchMoviesAsync(title))
                            .ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await controller.Search(title);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void GetSearchHistory_Returns_OK_With_Search_History()
        {
            // Arrange
            var mockMovieService = new Mock<IMovieService>();
            var mockSearchHistoryService = new Mock<ISearchHistoryService>();
            var controller = new MovieController(mockMovieService.Object, mockSearchHistoryService.Object);
            var searchHistory = new List<string> { "Avatar", "Inception" };
            mockSearchHistoryService.Setup(service => service.GetSearchHistory())
                                    .Returns(searchHistory);

            // Act
            var result = controller.GetSearchHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSearchHistory = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Equal(searchHistory, returnedSearchHistory);
        }

        [Fact]
        public void GetSearchHistory_Returns_InternalServerError_On_Exception()
        {
            // Arrange
            var mockMovieService = new Mock<IMovieService>();
            var mockSearchHistoryService = new Mock<ISearchHistoryService>();
            var controller = new MovieController(mockMovieService.Object, mockSearchHistoryService.Object);
            mockSearchHistoryService.Setup(service => service.GetSearchHistory())
                                    .Throws(new Exception("An error occurred"));

            // Act
            var result = controller.GetSearchHistory();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
