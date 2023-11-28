using Reddit.Data.Contracts;
using RedditSharp.Things;

namespace PostRetriever.WorkerService.Services;

public interface IMapper<TRedditSharp, TData>
{
    TData Map(TRedditSharp input);
}

public class PostMapper : IMapper<Post, IRedditPost>
{
    private readonly IRedditPosts _redditPosts;

    public PostMapper(IRedditPosts redditPosts)
    {
        _redditPosts = redditPosts;
    }

    public IRedditPost Map(Post input)
    {
        var redditPost = new RedditPost(_redditPosts);
        
        redditPost.Id = input.Id;
        redditPost.Created = input.CreatedUTC;
        redditPost.AuthorName = input.AuthorName;
        redditPost.UpVotes = input.Upvotes;
        
        return redditPost;
    }
}