using Reddit.Data.Contracts;
using StatisticsAPI.Models;

namespace StatisticsAPI.Mappers;

public interface IMapper<TDomain, TData>
{
    TDomain Map(TData input);
}

public class PostMapper : IMapper<Post, IRedditPost>
{
    public Post Map(IRedditPost input)
    {
        var post = new Post();
        post.Id = input.Id;
        post.AuthorName = input.AuthorName;
        post.UpVotes = input.UpVotes;
        post.Created = input.Created;
        return post;
    }
}