namespace PostRetriever.WorkerService.Services;

public class RedditRequestHandler : DelegatingHandler
{
    private readonly IRedditAuthenticator _redditAuthenticator;

    public RedditRequestHandler(IRedditAuthenticator redditAuthenticator)
    {
        _redditAuthenticator = redditAuthenticator;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return base.SendAsync(request, cancellationToken).GetAwaiter().GetResult();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authToken = await _redditAuthenticator.GetAuthToken();
        request.Headers.Add("Authorization", $"Bearer {authToken!.AccessToken}");
        return await base.SendAsync(request, cancellationToken);
    }
}

