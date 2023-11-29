using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Reddit.Data.Contracts;
using StatisticsAPI.Controllers;
using StatisticsAPI.Models;
using StatisticsAPI.Services;

namespace StatisticsAPI.Tests.Unit.ControllerTests
{
    public class UsersControllerTests
    {
        private static readonly Fixture Fixture = new();

        private readonly IUsersService _usersService = Substitute.For<IUsersService>();
        private readonly ILoggerWrapper<UsersController> _loggerWrapper = Substitute.For<ILoggerWrapper<UsersController>>();

        private int _top;
        private readonly IEnumerable<User> _users = Fixture.CreateMany<User>();
        private readonly Exception _exception = Fixture.Create<Exception>();

        private IResult _result = null!;

        [Fact]
        public void ByPosts_GivenTopBelowMinimum_ShouldReturnBadRequest()
        {
            // Arrange
            GivenTopIsBelowMinimum();

            // Act
            WhenByPostsIsCalled();

            // Assert
            ThenBadRequestReturned();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByPosts_GivenTopAboveMaximum_ShouldReturnBadRequest()
        {
            // Arrange
            GivenTopIsAboveMaximum();

            // Act
            WhenByPostsIsCalled();

            // Assert
            ThenBadRequestReturned();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByPosts_GivenAnExceptionIsThrown_ShouldReturnInternalServerError()
        {
            // Arrange
            GivenAnInRangeTop();
            GivenAnExceptionIsThrown();

            // Act
            WhenByPostsIsCalled();

            // Assert
            ThenExceptionWasLogged();
            ThenInternalServerErrorWasReturned();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByPosts_GivenNoPostsFound_ShouldReturnEmptyOkResponse()
        {
            // Arrange
            GivenAnInRangeTop();
            GivenNoPosts();

            // Act
            WhenByPostsIsCalled();

            // Assert
            ThenEmptyOkResponse();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void ByPosts_GivenPostsFound_ShouldReturnOkResponseWithPosts()
        {
            // Arrange
            GivenAnInRangeTop();
            GivenSomePosts();

            // Act
            WhenByPostsIsCalled();
            
            // Assert
            ThenOkResponseWithPostsReturned();
        }

        private void GivenSomePosts()
        {
            _usersService.GetTopPosters(_top).Returns(_users);
        }

        private void ThenOkResponseWithPostsReturned()
        {
            _result.Should().BeOfType<Ok<IEnumerable<User>>>();
            ((Ok<IEnumerable<User>>)_result).Value.Should().BeEquivalentTo(_users);
        }

        private void GivenNoPosts()
        {
            _usersService.GetTopPosters(_top).Returns(Enumerable.Empty<User>());
        }

        private void ThenEmptyOkResponse()
        {
            _result.Should().BeOfType<Ok<IEnumerable<User>>>();
            ((Ok<IEnumerable<User>>)_result).Value.Should().BeEquivalentTo(Enumerable.Empty<IRedditPost>());
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
            _usersService.GetTopPosters(Arg.Any<int>()).Throws(_exception);
        }

        private void WhenByPostsIsCalled()
        {
            var target = new UsersController(_usersService, _loggerWrapper);
            _result = target.ByPosts(_top);
        }

        private void ThenBadRequestReturned()
        {
            _result.Should().BeOfType<BadRequest<string>>();
            ((BadRequest<string>)_result).Value.Should().Be("Invalid number of users requested");
        }

        private void ThenExceptionWasLogged()
        {
            _loggerWrapper.Received().LogError(_exception, "Unexpected Error Retrieving {Class}.{Method}", nameof(UsersController), nameof(UsersController.ByPosts));
        }

        private void ThenInternalServerErrorWasReturned()
        {
            _result.Should().BeOfType<ProblemHttpResult>();
            ((ProblemHttpResult)_result).StatusCode.Should().Be(500);
        }
    }
}