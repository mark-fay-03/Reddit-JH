using FluentAssertions;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NSubstitute;

namespace PostRetriever.WorkerService.Tests.Unit;

public class OAuthTokenCacheTests
{
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();

    private const string AUTH_TOKEN_STRING = "{\"access_token\":\"MTQ0NjJkZmQ5OTM2NDE1ZTZjNGZmZjI3\", \"token_type\":\"Bearer\", \"expires_in\":3600, \"refresh_token\":\"IwOGYzYTlmM2YxOTQ5MGE3YmNmMDFkNTVk\", \"scope\":\"create\"}";
    private static readonly OAuthTokenResponse AuthToken = OAuthTokenResponse.Success(JObject.Parse(AUTH_TOKEN_STRING));

    private readonly Task<OAuthTokenResponse?> _entry = Task.FromResult(AuthToken)!;
    private const string CACHE_KEY = "StrongBad";
    private OAuthTokenResponse? _result;

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GetOrCreate_ShouldPassToInMemoryCache()
    {
        // Act
        await WhenGetOrCreateIsCalled();

        // Assert
        ThenEntryWrittenToInMemoryCache();
        ThenAuthTokenReturned();
    }

    private void ThenEntryWrittenToInMemoryCache()
    {
        _memoryCache.Received().GetOrCreateAsync(CACHE_KEY, _ => _entry);
    }

    private async Task WhenGetOrCreateIsCalled()
    {
        ICache<OAuthTokenResponse> target = new OAuthTokenCache(_memoryCache);
        _result = await target.GetOrCreate(CACHE_KEY, _ => _entry);
    }

    private void ThenAuthTokenReturned()
    {
        _result.Should().Be(AuthToken);
    }
}