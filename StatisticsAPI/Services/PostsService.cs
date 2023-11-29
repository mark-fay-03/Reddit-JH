using Reddit.Data.Contracts;
using StatisticsAPI.Models;

namespace StatisticsAPI.Services;

public interface IPostsService
{
    IEnumerable<Post> GetTopVoted(int top);
}

public class PostsService : IPostsService
{
    private readonly IRedditPosts _redditPosts;
    private readonly Mappers.IMapper<Post, IRedditPost> _mapper;

    public PostsService(IRedditPosts redditPosts, Mappers.IMapper<Post, IRedditPost> mapper)
    {
        _redditPosts = redditPosts;
        _mapper = mapper;
    }

    public IEnumerable<Post> GetTopVoted(int top)
    {
        var redditPosts = _redditPosts.Get();
        var orderedPosts = redditPosts.OrderByDescending(p => p.UpVotes);
        var topVotedPosts = orderedPosts.Take(top);
        var posts = topVotedPosts.Select(_mapper.Map);
        return posts;
    }
}