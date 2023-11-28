using Microsoft.AspNetCore.Authentication.OAuth;
using PostRetriever.WorkerService.Services;

namespace PostRetriever.WorkerService;

public static class ServicesRegistrar
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration contextConfiguration)
    {
        services.Configure<RedditAuthConfig>(contextConfiguration.GetSection(nameof(RedditAuthConfig)));

        services.AddSingleton<IRedditDataProvider, RedditDataProvider>();

        services.AddSingleton<IRedditAuthenticator, RedditBasicAuthenticator>();
        services.AddSingleton<ICache<OAuthTokenResponse>, OAuthTokenCache>();
        services.AddMemoryCache();
        services.AddSingleton<RedditRequestHandler>();
        services.AddHttpClient<RedditDataProvider>().AddHttpMessageHandler<RedditRequestHandler>();

        return services;
    }
}