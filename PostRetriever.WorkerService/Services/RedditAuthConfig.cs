namespace PostRetriever.WorkerService.Services;

public class RedditAuthConfig
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Uri TokenEndpoint { get; set; } = null!;
}