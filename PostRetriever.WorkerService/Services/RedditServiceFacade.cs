using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace PostRetriever.WorkerService.Services;
public interface IRedditServiceFacade
{
    Task<OAuthTokenResponse?> GetAuthenticationToken();
}

public class RedditServiceFacade : IRedditServiceFacade
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RedditAuthConfig _redditAuthConfig;

    public RedditServiceFacade(IHttpClientFactory httpClientFactory, IOptions<RedditAuthConfig> redditAuthConfigOptions)
    {
        _redditAuthConfig = redditAuthConfigOptions.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OAuthTokenResponse?> GetAuthenticationToken()
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(RedditServiceFacade));

        var requestContent = new StringContent($"grant_type=password&username={_redditAuthConfig.UserName}&password={_redditAuthConfig.Password}", Encoding.UTF8, "application/x-www-form-urlencoded");

        var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_redditAuthConfig.ClientId}:{_redditAuthConfig.ClientSecret}"));
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeaderValue}");

        var response = await httpClient.PostAsync(_redditAuthConfig.TokenEndpoint, requestContent);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        
        var authToken = OAuthTokenResponse.Success(JObject.Parse(responseBody));
        return authToken;
    }
}