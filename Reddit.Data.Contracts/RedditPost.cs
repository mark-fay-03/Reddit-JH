namespace Reddit.Data.Contracts;

public class RedditPost
{
    public string Id { get; set; } = null!;
    public DateTime Created { get; set; }
    public string AuthorName { get; set; } = null!;
    public int UpVotes { get; set; }
}