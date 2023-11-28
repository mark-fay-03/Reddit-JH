using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NSubstitute;
using PostRetriever.WorkerService.Services;

namespace PostRetriever.WorkerService.Tests.Unit;

public class RedditBasicAuthenticatorTests
{
    private readonly ICache<OAuthTokenResponse> _cache = Substitute.For<ICache<OAuthTokenResponse>>();
    private readonly OAuthTokenResponse? _authToken = OAuthTokenResponse.Success(new JObject());
    private readonly IRedditServiceFacade _redditServiceFacade = Substitute.For<IRedditServiceFacade>();
    private readonly IOptions<RedditAuthConfig> _configOptions = Options.Create(RedditAuthConfig);
    private static readonly RedditAuthConfig RedditAuthConfig = new Fixture().Create<RedditAuthConfig>();
    private OAuthTokenResponse? _result;
    private readonly ICacheEntry _cacheEntry = Substitute.For<ICacheEntry>();

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldGetOrCreateAuthTokenByKey()
    {
        // Arrange
        GivenCacheContainsKey();

        // Act
        await WhenGetAuthTokenIsCalled();

        // Assert
        ThenAuthTokenReturned();
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task CreateCacheEntry_ShouldRetrieveAccessToken()
    {
        // Arrange
        GivenAnAuthenticationTokenIsRetrieved();

        // Act
        await WhenCreateCacheEntryIsCalled();

        // Assert
        ThenAuthTokenReturned();
        ThenCacheWasUpdated();
    }


    private void GivenCacheContainsKey()
    {
        _cache.GetOrCreate($"{RedditAuthConfig.ClientId}:{RedditAuthConfig.UserName}", Arg.Any<Func<ICacheEntry, Task<OAuthTokenResponse?>>>()).Returns(_authToken);
    }

    private void GivenAnAuthenticationTokenIsRetrieved()
    {
        _redditServiceFacade.GetAuthenticationToken().Returns(_authToken);
    }

    private async Task WhenGetAuthTokenIsCalled()
    { 
        var target = new RedditBasicAuthenticator(_configOptions, _cache, _redditServiceFacade);
        _result = await target.GetAuthToken();
    }

    private async Task WhenCreateCacheEntryIsCalled()
    { 
        var target = new RedditBasicAuthenticator(_configOptions, _cache, _redditServiceFacade);
        _result = await target.CreateCacheEntry(_cacheEntry);
    }

    private void ThenAuthTokenReturned()
    {
        _result.Should().Be(_authToken);
    }

    private void ThenCacheWasUpdated()
    {
        _cacheEntry.Value.Should().Be(_authToken);
        _cacheEntry.AbsoluteExpirationRelativeToNow!.Value.Should().Be(TimeSpan.FromHours(1));
    }
}