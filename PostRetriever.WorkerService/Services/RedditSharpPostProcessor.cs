using PostRetriever.WorkerService.Wrappers;
using RedditSharp.Things;

namespace PostRetriever.WorkerService.Services;
public interface IRedditPostProcessor
{
    public Task StreamPosts(string subRedditId, IObserver<Post> observer, CancellationToken cancellationToken);
}

public class RedditSharpPostProcessor : IRedditPostProcessor
{
    private readonly IRedditWrapper _redditWrapper;

    public RedditSharpPostProcessor(IRedditWrapper redditWrapper)
    {
        _redditWrapper = redditWrapper;
    }

    public async Task StreamPosts(string subRedditId, IObserver<Post> observer, CancellationToken cancellationToken)
    {
        var subReddit = await _redditWrapper.GetSubRedditAsync($"/r/{subRedditId}");
        
        var postsStream = subReddit.GetPosts(Subreddit.Sort.New).Stream();
        using (postsStream.Subscribe(observer))
        {
            await postsStream.Enumerate(cancellationToken);
        }
    }
}