﻿using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace PostRetriever.WorkerService.Services;
public interface IRedditDataProvider
{
    Task<OAuthTokenResponse?> GetAuthenticationToken();
}

public class RedditDataProvider : IRedditDataProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RedditAuthConfig _redditAuthConfig;

    public RedditDataProvider(IHttpClientFactory httpClientFactory, IOptions<RedditAuthConfig> redditAuthConfigOptions)
    {
        _redditAuthConfig = redditAuthConfigOptions.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OAuthTokenResponse?> GetAuthenticationToken()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", _redditAuthConfig.UserName),
            new KeyValuePair<string, string>("password", _redditAuthConfig.Password)
        });
        httpClient.DefaultRequestHeaders.Add("User-Agent", _redditAuthConfig.UserAgent);

        var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_redditAuthConfig.ClientId}:{_redditAuthConfig.ClientSecret}"));
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeaderValue}");

        var response = await httpClient.PostAsync(_redditAuthConfig.TokenEndpoint, requestContent);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        
        var authToken = OAuthTokenResponse.Success(JObject.Parse(responseBody));
        return authToken;
    }
}