using RedditSharp;

namespace PostRetriever.WorkerService.Wrappers;

public interface IRedditWrapper
{
    Task<ISubRedditWrapper> GetSubRedditAsync(string name, bool validateName = true);
}

public class RedditWrapper : IRedditWrapper
{
    private readonly Reddit _redditApi;

    public RedditWrapper(IWebAgent webAgent, bool initUser)
    {
        _redditApi = new Reddit(webAgent, initUser);
    }

    public async Task<ISubRedditWrapper> GetSubRedditAsync(string name, bool validateName = true)
    {
        return new SubRedditWrapper(await _redditApi.GetSubredditAsync(name, validateName));
    }
}