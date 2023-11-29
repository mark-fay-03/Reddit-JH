namespace StatisticsAPI.Models;

public record User
{
    public string AuthorName { get; set; } = null!;
    public int PostCount { get; set; }
}