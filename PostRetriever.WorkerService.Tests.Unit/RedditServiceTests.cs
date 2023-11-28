using System.Net;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NSubstitute;
using PostRetriever.WorkerService.Services;

namespace PostRetriever.WorkerService.Tests.Unit;

public class RedditServiceTests
{
    private static readonly Fixture Fixture = new();

    private const string AUTH_TOKEN_STRING = "{\"access_token\":\"MTQ0NjJkZmQ5OTM2NDE1ZTZjNGZmZjI3\", \"token_type\":\"Bearer\", \"expires_in\":3600, \"refresh_token\":\"IwOGYzYTlmM2YxOTQ5MGE3YmNmMDFkNTVk\", \"scope\":\"create\"}";
    private readonly OAuthTokenResponse _authToken = OAuthTokenResponse.Success(JObject.Parse(AUTH_TOKEN_STRING));
    private const string INVALID_AUTH_RESPONSE = "{\"Trogdor\": \"Da Burninator\"}";

    private static readonly RedditAuthConfig RedditAuthConfig = Fixture.Create<RedditAuthConfig>();
    private readonly IOptions<RedditAuthConfig> _redditAuthConfigOptions = Options.Create(RedditAuthConfig);

    private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
    private readonly MockHttpMessageHandler _mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
    private HttpClient _httpClient = null!;
    private HttpResponseMessage _response = null!;

    private OAuthTokenResponse? _result;
    private Func<Task> _action = null!;


    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldReturnSuccessfulAuthToken()
    {
        // Arrange
        GivenResponseIsSuccessful();
        GivenServerReturnsAResponse();
        GivenAnHttpClient();
        GivenResponseContainsASuccessfulOAuthToken();

        // Act
        await WhenGetAuthenticationTokenIsCalled();

        // Assert
        ThenAuthTokenIsReturned();
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GivenRequestFails_ShouldThrowException()
    {
        // Arrange
        GivenResponseIsNotSuccessful();
        GivenServerReturnsAResponse();
        GivenAnHttpClient();
        
        // Act
        WhenGetAuthenticationTokenThrowsAnException();

        // Assert
        await ThenHttpRequestExceptionIsThrown();
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GivenResponseContainsInvalidContent_ShouldReturnNull()
    {
        // Arrange
        GivenResponseIsSuccessful();
        GivenServerReturnsAResponse();
        GivenAnHttpClient();
        GivenResponseContentDoesNotContainOAuthToken();

        // Act
        await WhenGetAuthenticationTokenIsCalled();

        // Assert
        ThenNullIsReturned();
    }


    private void GivenAnHttpClient()
    {
        _httpClient = new HttpClient(_mockHttpMessageHandler);
        _httpClientFactory.CreateClient().Returns(_httpClient);
    }

    private void GivenResponseIsSuccessful()
    {
        _response = new HttpResponseMessage(HttpStatusCode.OK);
    }

    private void GivenResponseIsNotSuccessful()
    {
        _response = new HttpResponseMessage(HttpStatusCode.Conflict);
    }

    private void GivenResponseContainsASuccessfulOAuthToken()
    {
        _response.Content = new StringContent(AUTH_TOKEN_STRING);
    }

    private void GivenResponseContentDoesNotContainOAuthToken()
    {
        _response.Content = new StringContent(INVALID_AUTH_RESPONSE);
    }

    private void GivenServerReturnsAResponse()
    {
        _mockHttpMessageHandler.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()).Returns(_response);
    }

    private async Task WhenGetAuthenticationTokenIsCalled()
    {
        var target = new RedditDataProvider(_httpClientFactory, _redditAuthConfigOptions);
        _result = await target.GetAuthenticationToken();
    }

    private void WhenGetAuthenticationTokenThrowsAnException()
    {
        _action = WhenGetAuthenticationTokenIsCalled;
    }

    private void ThenAuthTokenIsReturned()
    {
        _result.Should().BeEquivalentTo(_authToken);
    }

    private async Task ThenHttpRequestExceptionIsThrown()
    {
        var thrownException = await _action.Should().ThrowAsync<HttpRequestException>();
        thrownException.Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    private void ThenNullIsReturned()
    {
        _result.Should().NotBeNull();
        _result!.AccessToken.Should().BeNull();
        _result.ExpiresIn.Should().BeNull();
        _result.Error.Should().BeNull();
        _result.RefreshToken.Should().BeNull();
        _result.TokenType.Should().BeNull();
        _result.Response.Should().BeEquivalentTo(JObject.Parse(INVALID_AUTH_RESPONSE));
    }
}

public class MockHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Send(request, cancellationToken);
    }

    public new virtual Task<HttpResponseMessage> Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}