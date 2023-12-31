﻿namespace PostRetriever.WorkerService.Services;

public class RedditAuthConfig
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string SubRedditName { get; set; } = null!;
}