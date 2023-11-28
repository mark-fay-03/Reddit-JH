using PostRetriever.WorkerService.Services;
using RedditSharp.Things;
using AutoFixture;
using FluentAssertions;

namespace PostRetriever.WorkerService.Tests.Unit;

public class PostMapperTests
{
    private static readonly Fixture Fixture = new();
    private readonly Post _redditSharpPost = Fixture.Create<Post>();
    
    private RedditPost _result = null!;

    [Fact]
    [Trait("Category", "UnitTest")]
    public void ShouldMapToDataType()
    {
        // Act
        WhenMapIsCalled();

        // Assert
        ThenRedditPostWasReturned();
    }

    private void ThenRedditPostWasReturned()
    {
        _result.Id.Should().Be(_redditSharpPost.Id);
        _result.AuthorName.Should().Be(_redditSharpPost.AuthorName);
        _result.Upvotes.Should().Be(_redditSharpPost.Upvotes);
        _result.Created.Should().Be(_redditSharpPost.CreatedUTC);
    }

    private void WhenMapIsCalled()
    {
        IMapper<Post, RedditPost> target = new PostMapper();
        _result = target.Map(_redditSharpPost);
    }
}