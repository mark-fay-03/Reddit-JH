using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Reddit.Data.Contracts;
using StatisticsAPI.Models;
using StatisticsAPI.Services;

namespace StatisticsAPI.Tests.Unit.ServiceTests;

public class UsersServiceTests
{
    private static readonly IRedditPosts _dataStore = Substitute.For<IRedditPosts>();
    private readonly IEnumerable<RedditPost> _redditPosts = new Fixture().Build<RedditPost>().FromFactory(() => new RedditPost(_dataStore)).CreateMany(100);
    private readonly Exception _exception = new Fixture().Create<Exception>();
    private Action _action = null!;
    private int _top = 69;
    private IEnumerable<User> _result = null!;

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetTopPosters_ShouldBubbleUpExceptions()
    {
        // Arrange
        GivenAnExceptionIsThrown();

        // Act
        WhenGetTopPostersThrowsAnException();
        
        // Assert
        ThenExceptionBubblesUp();
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetTopPosters_ShouldReturnTopUsersByNumberOfPosts()
    {
        // Arrange
        GivenSomePosts();

        // Act
        WhenGetTopPostersIsCalled();

        // Assert
        ThenUsersWithTheMostPostsAreReturned();
    }

    private void GivenAnExceptionIsThrown()
    {
        _dataStore.Get().Throws(_exception);
    }

    private void GivenSomePosts()
    {
        _dataStore.Get().Returns(_redditPosts);
    }

    private void WhenGetTopPostersIsCalled()
    {
        IUsersService target = new UsersService(_dataStore);
        _result = target.GetTopPosters(_top);
    }

    private void WhenGetTopPostersThrowsAnException()
    {
        _action = WhenGetTopPostersIsCalled;
    }

    private void ThenExceptionBubblesUp()
    {
        _action.Should().Throw<Exception>().Which.Should().Be(_exception);
    }

    private void ThenUsersWithTheMostPostsAreReturned()
    {
        var expectedUsers = _redditPosts.GroupBy(post => post.AuthorName)
            .OrderByDescending(group => group.Count())
            .Take(_top)
            .Select(group => new User { AuthorName = group.Key, PostCount = group.Count() });
        _result.Should().BeEquivalentTo(expectedUsers);
    }
}