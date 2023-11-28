using RedditSharp.Things;

namespace PostRetriever.WorkerService.Services;

public interface IMapper<TRedditSharp, TData>
{
    TData Map(TRedditSharp input);
}

public class PostMapper : IMapper<Post, RedditPost>
{
    public RedditPost Map(Post input)
    {
        var redditPost = new RedditPost();
        
        redditPost.Id = input.Id;
        redditPost.Created = input.CreatedUTC;
        redditPost.AuthorName = input.AuthorName;
        redditPost.Upvotes = input.Upvotes;
        
        return redditPost;
    }
}