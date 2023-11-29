using System.Collections.Concurrent;
using System.Reflection;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Reddit.Data.Contracts;

namespace Reddit.Data.InMemory.Tests.Unit.RedditPostsTests;

public class GetTests
{
    readonly IRedditPosts _target = new RedditPosts();

    private readonly IEnumerable<IRedditPost> _posts = new Fixture().Build<RedditPost>().FromFactory(() => new RedditPost(Substitute.For<IRedditPosts>())).CreateMany(5);
    private IEnumerable<IRedditPost> _result = null!;
    
    [Fact]
    [Trait("Category", "UnitTest")]
    public void ShouldReturnPosts()
    {
        // Arrange
        GivenSomePostsInTheDataStore();

        // Act
        WhenGetIsCalled();

        // Assert
        ThenRedditPostsWereReturned();
    }

    private void GivenSomePostsInTheDataStore()
    {
        var postsField = typeof(RedditPosts).GetField("_posts", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
        postsField!.SetValue(_target, new ConcurrentBag<IRedditPost>(_posts));
    }

    private void WhenGetIsCalled()
    {
        _result = _target.Get();
    }

    private void ThenRedditPostsWereReturned()
    {
        _result.Should().BeEquivalentTo(_posts);
    }
}