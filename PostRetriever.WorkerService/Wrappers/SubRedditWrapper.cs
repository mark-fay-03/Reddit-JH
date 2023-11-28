using RedditSharp.Things;

namespace PostRetriever.WorkerService.Wrappers;

public interface ISubRedditWrapper
{
    IListingWrapper<Post> GetPosts(Subreddit.Sort sort, int max = -1);
}

public class SubRedditWrapper : ISubRedditWrapper
{
    private readonly Subreddit _subReddit;

    public SubRedditWrapper(Subreddit subReddit)
    {
        _subReddit = subReddit;
    }

    public IListingWrapper<Post> GetPosts(Subreddit.Sort sort, int max = -1)
    {
        return new ListingWrapper<Post>(_subReddit.GetPosts(sort, max));
    }
}