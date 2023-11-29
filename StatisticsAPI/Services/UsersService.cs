using Reddit.Data.Contracts;
using StatisticsAPI.Models;

namespace StatisticsAPI.Services;

public interface IUsersService
{
    IEnumerable<User> GetTopPosters(int top);
}

public class UsersService : IUsersService
{
    private readonly IRedditPosts _redditPosts;

    public UsersService(IRedditPosts redditPosts)
    {
        _redditPosts = redditPosts;
    }

    public IEnumerable<User> GetTopPosters(int top)
    {
        var topAuthors = _redditPosts.Get()
            .GroupBy(post => post.AuthorName)
            .OrderByDescending(group => group.Count())
            .Take(top)
            .Select(group => new User { AuthorName = group.Key, PostCount = group.Count() });
        return topAuthors;
    }
}