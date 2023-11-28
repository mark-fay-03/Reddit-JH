namespace Reddit.Data.Contracts;

public interface IRedditPost
{
    string Id { get; set; }
    DateTime Created { get; set; }
    string AuthorName { get; set; }
    int UpVotes { get; set; }
    Task Save();
}

public class RedditPost : IRedditPost
{
    private readonly IRedditPosts _redditPosts;

    public RedditPost(IRedditPosts redditPosts)
    {
        _redditPosts = redditPosts;
    }

    public string Id { get; set; } = null!;
    public DateTime Created { get; set; }
    public string AuthorName { get; set; } = null!;
    public int UpVotes { get; set; }

    public async Task Save()
    {
        await _redditPosts.Add(this);
    }
}