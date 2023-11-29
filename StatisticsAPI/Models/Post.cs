namespace StatisticsAPI.Models;

public class Post
{
    public string Id { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public int UpVotes { get; set; }
    public DateTime Created { get; set; }
}