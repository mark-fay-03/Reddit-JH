using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using StatisticsAPI.Controllers;
using StatisticsAPI.Models;
using StatisticsAPI.Services;

namespace StatisticsAPI.Tests.Unit.ControllerTests
{
    public class PostsControllerTests
    {
        private static readonly Fixture Fixture = new();

        private IResult _result = null!;
        private readonly IPostsService _postsService = Substitute.For<IPostsService>();
        private readonly ILoggerWrapper<PostsController> _loggerWrapper = Substitute.For<ILoggerWrapper<PostsController>>();
        
        private int _top;
        private readonly Exception _exception = Fixture.Create<Exception>();
        private readonly IEnumerable<Post> _posts = Fixture.CreateMany<Post>();

        [Fact]
        public void ByVotes_GivenTopBelowMinimum_ShouldReturnBadRequest()
        {
            // Arrange
            GivenTopIsBelowMinimum();

            // Act
            WhenByVotesIsCalled();

            // Assert
            ThenBadRequestReturned();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByVotes_GivenTopAboveMaximum_ShouldReturnBadRequest()
        {
            // Arrange
            GivenTopIsAboveMaximum();

            // Act
            WhenByVotesIsCalled();

            // Assert
            ThenBadRequestReturned();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByVotes_GivenAnExceptionIsThrown_ShouldReturnInternalServerError()
        {
            // Arrange
            GivenAnInRangeTop();
            GivenAnExceptionIsThrown();

            // Act
            WhenByVotesIsCalled();

            // Assert
            ThenExceptionWasLogged();
            ThenInternalServerErrorWasReturned();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByVotes_GivenNoPostsFound_ShouldReturnEmptyOkResponse()
        {
            // Arrange
            GivenAnInRangeTop();
            GivenNoPosts();

            // Act
            WhenByVotesIsCalled();

            // Assert
            ThenEmptyOkResponse();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByVotes_GivenPostsFound_ShouldReturnOkResponseWithPosts()
        {
            // Arrange
            GivenAnInRangeTop();
            GivenSomePosts();

            // Act
            WhenByVotesIsCalled();
            
            // Assert
            ThenOkResponseWithPostsReturned();
        }

        private void GivenSomePosts()
        {
            _postsService.GetTopVoted(_top).Returns(_posts);
        }

        private void ThenOkResponseWithPostsReturned()
        {
            _result.Should().BeOfType<Ok<IEnumerable<Post>>>();
            ((Ok<IEnumerable<Post>>)_result).Value.Should().BeEquivalentTo(_posts);
        }

        private void GivenNoPosts()
        {
            _postsService.GetTopVoted(_top).Returns(Enumerable.Empty<Post>());
        }

        private void ThenEmptyOkResponse()
        {
            _result.Should().BeOfType<Ok<IEnumerable<Post>>>();
            ((Ok<IEnumerable<Post>>)_result).Value.Should().BeEquivalentTo(Enumerable.Empty<Post>());
        }
        
        private void GivenTopIsBelowMinimum()
        {
            _top = 0;
        }

        private void GivenTopIsAboveMaximum()
        {
            _top = 101;
        }

        private void GivenAnInRangeTop()
        {
            _top = 10;
        }

        private void GivenAnExceptionIsThrown()
        {
            _postsService.GetTopVoted(Arg.Any<int>()).Throws(_exception);
        }

        private void WhenByVotesIsCalled()
        {
            var target = new PostsController(_postsService, _loggerWrapper);
            _result = target.ByVotes(_top);
        }

        private void ThenBadRequestReturned()
        {
            _result.Should().BeOfType<BadRequest<string>>();
            ((BadRequest<string>)_result).Value.Should().Be("Invalid number of users requested");
        }

        private void ThenExceptionWasLogged()
        {
            _loggerWrapper.Received().LogError(_exception, "Unexpected Error Retrieving {Class}.{Method}", nameof(PostsController), nameof(PostsController.ByVotes));
        }

        private void ThenInternalServerErrorWasReturned()
        {
            _result.Should().BeOfType<ProblemHttpResult>();
            ((ProblemHttpResult)_result).StatusCode.Should().Be(500);
        }
    }
}