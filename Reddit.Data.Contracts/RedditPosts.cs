using System.Collections.Concurrent;

namespace Reddit.Data.Contracts;

public interface IRedditPosts
{
    Task Add(IRedditPost redditPost);
}

public class RedditPosts : IRedditPosts
{
    private readonly ConcurrentBag<IRedditPost> _posts = new();

    public Task Add(IRedditPost redditPost)
    {
        _posts.Add(redditPost);
        return Task.CompletedTask;
    }
}