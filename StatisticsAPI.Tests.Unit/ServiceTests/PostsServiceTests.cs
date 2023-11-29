using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Reddit.Data.Contracts;
using StatisticsAPI.Mappers;
using StatisticsAPI.Models;
using StatisticsAPI.Services;

namespace StatisticsAPI.Tests.Unit.ServiceTests;

public class PostsServiceTests
{
    private static readonly IRedditPosts DataStore = Substitute.For<IRedditPosts>();
    private readonly IMapper<Post, IRedditPost> _mapper = Substitute.For<IMapper<Post, IRedditPost>>();

    private const int TOP = 10;

    private readonly IEnumerable<IRedditPost> _redditPosts = new Fixture()
        .Build<RedditPost>()
        .FromFactory(() => new RedditPost(DataStore))
        .CreateMany(TOP * 3);
    private readonly IEnumerable<Post> _posts = new Fixture().CreateMany<Post>(TOP).OrderByDescending(p => p.UpVotes);
    private readonly Exception _exception = new Fixture().Create<Exception>();

    private IEnumerable<Post> _result = null!;
    private Action _action = null!;

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetTopVoted_ShouldBubbleUpExceptions()
    {
        // Arrange
        GivenAnExceptionIsThrown();

        // Act
        WhenGetTopVotedThrowsAnException();

        // Assert
        ThenExceptionIsBubbledUp();
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetTopVoted_ShouldReturnTopVotedPosts()
    {
        // Arrange
        GivenSomeRedditPosts();
        GivenDataPostsAreConvertedToDomainPosts();

        // Act
        WhenGetTopVotedIsCalled();

        // Assert
        ThenPostsAreReturned();
    }

    private void GivenSomeRedditPosts()
    {
        DataStore.Get().Returns(_redditPosts);
    }

    private void GivenDataPostsAreConvertedToDomainPosts()
    {
        var topRedditPosts = _redditPosts.OrderByDescending(p => p.UpVotes).Take(TOP).ToArray();
        var mappedTopRedditPosts = _posts.ToArray();
        
        for (int i = 0; i < TOP; i++ )
        {
            _mapper.Map(topRedditPosts[i]).Returns(mappedTopRedditPosts[i]);
        }
    }

    private void ThenPostsAreReturned()
    {
        _result.Should().BeEquivalentTo(_posts.OrderByDescending(p => p.UpVotes).Take(TOP));
    }
    
    private void GivenAnExceptionIsThrown()
    {
        DataStore.Get().Throws(_exception);
    }

    private void WhenGetTopVotedIsCalled()
    {
        IPostsService target = new PostsService(DataStore, _mapper);
        _result = target.GetTopVoted(TOP);
    }

    private void WhenGetTopVotedThrowsAnException()
    {
        _action = WhenGetTopVotedIsCalled;
    }

    private void ThenExceptionIsBubbledUp()
    {
        _action.Should().Throw<Exception>().Which.Should().Be(_exception);
    }
}