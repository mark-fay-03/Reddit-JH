using System.Threading.Tasks;
using NSubstitute;
using PostRetriever.WorkerService.Services;
using PostRetriever.WorkerService.Wrappers;
using RedditSharp.Things;
using Xunit;

namespace PostRetriever.WorkerService.Tests.Unit;

public class RedditSharpPostProcessorTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private readonly IObserver<Post> _postObserver = null!;

    private readonly IRedditWrapper _redditWrapper = Substitute.For<IRedditWrapper>();
    private readonly ISubRedditWrapper _subRedditWrapper = Substitute.For<ISubRedditWrapper>();
    private readonly IListingWrapper<Post> _listingWrapper = Substitute.For<IListingWrapper<Post>>();
    private readonly IListingStreamWrapper<Post> _listingStreamWrapper = Substitute.For<IListingStreamWrapper<Post>>();

    private const string SUB_REDDIT_ID = "Trogdor";

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldCallPostObserver()
    {
        GivenTheSubRedditIs($"/r/{SUB_REDDIT_ID}");
        GivenSubRedditHasPost();
        GivenPostsCanBeStreamed();

        // Act
        await WhenStreamPostsIsCalled();

        // Assert
        ThenPostsAreProcessed();
    }

    private void GivenTheSubRedditIs(string subRedditId)
    {
        _redditWrapper.GetSubRedditAsync(subRedditId).Returns(_subRedditWrapper);
    }

    private void GivenSubRedditHasPost()
    {
        _subRedditWrapper.GetPosts(Subreddit.Sort.New).Returns(_listingWrapper);
    }

    private void GivenPostsCanBeStreamed()
    {
        _listingWrapper.Stream().Returns(_listingStreamWrapper);
    }

    private async Task WhenStreamPostsIsCalled()
    {
        IRedditPostProcessor target = new RedditSharpPostProcessor(_redditWrapper);
        await target.StreamPosts(SUB_REDDIT_ID, _postObserver, _cancellationToken);
    }

    private void ThenPostsAreProcessed()
    {
        _redditWrapper.Received().GetSubRedditAsync($"/r/{SUB_REDDIT_ID}");
        _subRedditWrapper.Received().GetPosts(Subreddit.Sort.New);
        _listingWrapper.Received().Stream();
        _listingStreamWrapper.Received().Subscribe(_postObserver);
        _listingStreamWrapper.Received().Enumerate(_cancellationToken);
    }
}