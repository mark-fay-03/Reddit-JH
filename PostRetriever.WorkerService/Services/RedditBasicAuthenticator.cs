using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace PostRetriever.WorkerService.Services;

public interface IRedditAuthenticator
{
    Task<OAuthTokenResponse?> GetAuthToken();
}

public class RedditBasicAuthenticator : IRedditAuthenticator
{
    private readonly ICache<OAuthTokenResponse> _oAuthTokenCache;
    private readonly RedditAuthConfig _redditAuthConfig;
    private readonly IRedditDataProvider _redditDataProvider;

    public RedditBasicAuthenticator(IOptions<RedditAuthConfig> redditAuthConfigOptions, ICache<OAuthTokenResponse> oAuthTokenCache, IRedditDataProvider redditDataProvider)
    {
        _oAuthTokenCache = oAuthTokenCache;
        _redditDataProvider = redditDataProvider;
        _redditAuthConfig = redditAuthConfigOptions.Value;
    }

    public async Task<OAuthTokenResponse?> GetAuthToken()
    {
        var accessToken = await _oAuthTokenCache.GetOrCreate($"{_redditAuthConfig.ClientId}:{_redditAuthConfig.UserName}", CreateCacheEntry);
        return accessToken;
    }

    internal async Task<OAuthTokenResponse?> CreateCacheEntry(ICacheEntry entry)
    {
        var authToken = await _redditDataProvider.GetAuthenticationToken();
        entry.Value = authToken;
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        return authToken;
    }
}