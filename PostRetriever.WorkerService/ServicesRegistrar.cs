using Microsoft.AspNetCore.Authentication.OAuth;
using PostRetriever.WorkerService.Services;

namespace PostRetriever.WorkerService;

public static class ServicesRegistrar
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration contextConfiguration)
    {
        services.Configure<RedditAuthConfig>(contextConfiguration.GetSection(nameof(RedditAuthConfig)));

        services.AddSingleton<IRedditServiceFacade, RedditServiceFacade>();

        services.AddSingleton<ICache<OAuthTokenResponse>, OAuthTokenCache>();
        services.AddMemoryCache();
        return services;
    }
}